using Microsoft.AspNetCore.Mvc;
using plato_backend.Model;
using plato_backend.Services;

namespace plato_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsServices _commentsServices;

        public CommentsController(CommentsServices commentsServices)
        {
            _commentsServices = commentsServices;
        }

        [HttpGet("GetAllComments")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentsServices.GetCommentsAsync();

            if (comments != null) return Ok(comments);

            return BadRequest(new {Message = "No Comments"});
        }

        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody]CommentsModel comment)
        {
            var success = await _commentsServices.AddCommentsAsync(comment);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Comment Was not Added"});
        }

        [HttpPut("EditComment")]
        public async Task<IActionResult> EditComment([FromBody]CommentsModel comment)
        {
            var success = await _commentsServices.EditCommentsAsync(comment);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Comment Failed To Update"});
        }

        [HttpDelete("DeleteComment")]
        public async Task<IActionResult> DeleteComment([FromBody]CommentsModel comment)
        {
            var success = await _commentsServices.EditCommentsAsync(comment);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Comment Failed To Update"});
        }

        [HttpGet("GetCommentsByUserId/{userId}")]
        public async Task<IActionResult> GetCommentsByUserId(int userId)
        {
            var comments = await _commentsServices.GetCommentsByUserIdAsync(userId);

            if (comments != null) return Ok(comments);

            return BadRequest(new {Message = "No Comments"});
        }

        [HttpGet("GetCommentsById/{id}")]
        public async Task<IActionResult> GetCommentsById(int id)
        {
            var comment = await _commentsServices.GetCommentsByIdAsync(id);

            if (comment != null) return Ok(comment);

            return BadRequest(new {Message = "No Comments"});
        }

        [HttpGet("GetCommentsByDate/{date}")]
        public async Task<IActionResult> GetCommentsByDate(string date)
        {
            var comments = await _commentsServices.GetCommentsByDateAsync(date);

            if (comments != null) return Ok(comments);

            return BadRequest(new {Message = "No Comments with that Date"});
        }

        [HttpGet("GetCommentsByBlogId/{id}")]
        public async Task<IActionResult> GetCommentsByBlogId(int id)
        {
            var comment = await _commentsServices.GetCommentsByBlogIdAsync(id);

            if (comment != null) return Ok(comment);

            return BadRequest(new {Message = "No Comments"});
        }
    }
}