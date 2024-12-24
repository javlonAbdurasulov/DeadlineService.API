using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.DTOs.Comment;

namespace DeadlineService.Application.Services
{

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        public CommentService(ICommentRepository commentRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;

        }
        public async Task<ResponseModel<IEnumerable<GetCommentDTO>>> GetComments()
        {
            var allOrders = await _orderRepository.GetAllAsync();

            var allComments = allOrders.Select(x => new GetCommentDTO()
            {
                SellerId = x.CreatedByUserId,
                FrilancerId = x.AssignedToUserId,
                Id = x.CommentId,
            });

            return new(allComments);
        }

        public async Task<ResponseModel<Comment>> PostComment(PostCommentDTO postCommentDTO)
        {

            var allUsers = await _userRepository.GetAllAsync();

            var seller = allUsers.FirstOrDefault(x => x.Id == postCommentDTO.SellerId);
            var frilancer = allUsers.FirstOrDefault(x => x.Id == postCommentDTO.FrilancerId);

            if (seller == null)
                return new("Такого заказчика не существует");

            else if (frilancer == null) return new("такого фрилансера не существует");

            var allOrder = await _orderRepository.GetAllAsync();
            var order = allOrder
                .FirstOrDefault(z => z.AssignedToUserId == postCommentDTO.FrilancerId && z.CreatedByUserId == postCommentDTO.SellerId);


            if (order == null)
                return new("Такого заказа не существует");
            Comment newComment = new Comment()
            {
                Text = postCommentDTO.Text,
                Order = order,
                OrderId = order.Id,
                Stars = postCommentDTO.Stars
            };
            var returnedComment = await _commentRepository.CreateAsync(newComment);

            return new(returnedComment);

        }

    }
}
