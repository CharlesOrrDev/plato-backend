using Microsoft.AspNetCore.Mvc;
using plato_backend.Model;
using plato_backend.Services;

namespace plato_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly ConversationServices _conversationServices;

        public ConversationController(ConversationServices conversationServices)
        {
            _conversationServices = conversationServices;
        }

        [HttpGet("GetAllConversations")]
        public async Task<IActionResult> GetAllConversations()
        {
            var conversations = await _conversationServices.GetAllConversationsAsync();

            if (conversations != null) return Ok(conversations);

            return BadRequest(new {Message = "No Conversations"});
        }

        [HttpPost("AddConversation")]
        public async Task<IActionResult> AddConversation([FromBody]ConversationModel conversation)
        {
            var success = await _conversationServices.AddConversationAsync(conversation);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Conversation Was not Added"});
        }

        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessage([FromBody]MessageModel message)
        {
            var success = await _conversationServices.AddMessageAsync(message);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Message Was not Added"});
        }

        [HttpGet("GetConversationById/{id}")]
        public async Task<IActionResult> GetConversationById(int id)
        {
            var conversation = await _conversationServices.GetConversationByIdAsync(id);

            if (conversation != null) return Ok(conversation);

            return BadRequest(new {Message = "No Conversation"});
        }

        [HttpGet("GetMessageById/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _conversationServices.GetMessageByIdAsync(id);

            if (message != null) return Ok(message);

            return BadRequest(new {Message = "No Message"});
        }

        [HttpGet("GetConversationsByUserOneId/{userOneId}")]
        public async Task<IActionResult> GetConversationsByUserOneId(int userOneId)
        {
            var conversation = await _conversationServices.GetConversationsByUserOneIdAsync(userOneId);

            if (conversation != null) return Ok(conversation);

            return BadRequest(new {Message = "No Conversations"});
        }

        [HttpGet("GetConversationsByUserTwoId/{userTwoId}")]
        public async Task<IActionResult> GetConversationsByUserTwoId(int userTwoId)
        {
            var conversation = await _conversationServices.GetConversationsByUserOneIdAsync(userTwoId);

            if (conversation != null) return Ok(conversation);

            return BadRequest(new {Message = "No Conversations"});
        }

        [HttpGet("GetMessageByUserId/{userId}")]
        public async Task<IActionResult> GetMessageByUserId(int userId)
        {
            var conversation = await _conversationServices.GetMessagesByUserIdAsync(userId);

            if (conversation != null) return Ok(conversation);

            return BadRequest(new {Message = "No Conversations"});
        }

        [HttpGet("GetMessagesByUserIdAndConversationId/{userId}/{conversationId}")]
        public async Task<IActionResult> GetMessagesByUserIdAndConversationId(int userId, int conversationId)
        {
            var messages = await _conversationServices.GetMessagesByUserIdAndConversationIdAsync(userId, conversationId);

            if (messages != null) return Ok(messages);

            return BadRequest(new {Message = "No Messages"});
        }

        [HttpPut("EditConversations")]
        public async Task<IActionResult> EditConversations([FromBody]ConversationModel conversation)
        {
            var success = await _conversationServices.EditConversationsAsync(conversation);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Conversation Failed To Update"});
        }

        [HttpPut("EditMessage")]
        public async Task<IActionResult> EditMessage([FromBody]MessageModel message)
        {
            var success = await _conversationServices.EditMessageAsync(message);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Message Failed To Update"});
        }
    }
}