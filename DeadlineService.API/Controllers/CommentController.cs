using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models;
using DeadlineService.Domain.Models.DTOs.Comment;
using DeadlineService.Domain.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace DeadlineService.API.Controllers
{
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IRedisCacheService _redisCache;
        public CommentController(ICommentService commentService,
            IRedisCacheService chacheService)
        {
            _redisCache = chacheService;  
            _commentService = commentService;
        }
        [HttpPost]
        public async Task<ResponseModel<Comment>> PostComment(PostCommentDTO comment)
        {
            return await _commentService.PostComment(comment);
        }
        [HttpGet]
        public async Task<ResponseModel<IEnumerable<GetCommentDTO>>> GetComments()
        {
            string cacheKey = "Comments";

            var cachedComments= await _redisCache.GetAsync(cacheKey);
            if(cachedComments != null)
            {
                var deserializedComments=JsonSerializer.Deserialize<IEnumerable<GetCommentDTO>>(cachedComments);
                return new(deserializedComments);
            }
            var comments = await _commentService.GetComments();
            var cached=JsonSerializer.Serialize(comments);
            await _redisCache.SetAsync(cacheKey, UTF8Encoding.UTF8.GetBytes(cached));
            return comments;
        }
    }
}
