using Microsoft.AspNetCore.Mvc;
using plato_backend.Model;
using plato_backend.Services;

namespace plato_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReplysController : ControllerBase
    {
        private readonly ReplysServices _replysServices;

        public ReplysController(ReplysServices replysServices)
        {
            _replysServices = replysServices;
        }

        [HttpGet("GetAllReplies")]
        public async Task<IActionResult> GetAllReplies()
        {
            var reply = await _replysServices.GetReplyAsync();

            if (reply != null) return Ok(reply);

            return BadRequest(new {Message = "No Replies"});
        }

        [HttpPost("AddReply")]
        public async Task<IActionResult> AddReply([FromBody]ReplysModel reply)
        {
            var success = await _replysServices.AddReplyAsync(reply);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Reply Was not Added"});
        }

        [HttpPut("EditReply")]
        public async Task<IActionResult> EditReply([FromBody]ReplysModel reply)
        {
            var success = await _replysServices.EditReplyAsync(reply);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Reply Failed To Update"});
        }

        [HttpDelete("DeleteReply")]
        public async Task<IActionResult> DeleteReply([FromBody]ReplysModel reply)
        {
            var success = await _replysServices.EditReplyAsync(reply);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Reply Failed To Update"});
        }

        [HttpGet("GetReplyByUserId/{userId}")]
        public async Task<IActionResult> GetCommentsByUserId(int userId)
        {
            var comments = await _replysServices.GetReplyByUserIdAsync(userId);

            if (comments != null) return Ok(comments);

            return BadRequest(new {Message = "No Replies"});
        }

        [HttpGet("GetReplyById/{id}")]
        public async Task<IActionResult> GetReplyById(int id)
        {
            var reply = await _replysServices.GetReplyByIdAsync(id);

            if (reply != null) return Ok(reply);

            return BadRequest(new {Message = "No Replies"});
        }

        [HttpGet("GetReplyByDate/{date}")]
        public async Task<IActionResult> GetReplyByDate(string date)
        {
            var reply = await _replysServices.GetReplyByDateAsync(date);

            if (reply != null) return Ok(reply);

            return BadRequest(new {Message = "No Replies with that Date"});
        }

        [HttpGet("GetReplyByCommentId/{id}")]
        public async Task<IActionResult> GetReplyByCommentId(int id)
        {
            var reply = await _replysServices.GetReplyByCommentIdAsync(id);

            if (reply != null) return Ok(reply);

            return BadRequest(new {Message = "No Comments"});
        }
    }
}