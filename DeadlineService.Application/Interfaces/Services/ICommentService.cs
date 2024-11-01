using DeadlineService.Domain.Models;
using DeadlineService.Domain.Models.DTOs.Comment;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface ICommentService
    {
        public Task<ResponseModel<IEnumerable<GetCommentDTO>>> GetComments();
        public Task<ResponseModel<Comment>> PostComment(PostCommentDTO postCommentDTO);
     }
}
