using Microsoft.AspNetCore.Mvc;
using plato_backend.Model;
using plato_backend.Model.DTOS;
using plato_backend.Services;

namespace plato_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]UserDTO user)
        {
            bool success = await _userServices.CreateAccount(user);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Success = false, Message = "Account creation Failed A User with the username or email Exists"});
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO user)
        {
            var success = await _userServices.Login(user);

            if (success != null) return Ok(new {Token = success});

            return Unauthorized(new {Message = "Login Failed Wrong Username/Email or Password"});
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userServices.GetUsersAsync();

            if (users != null) return Ok(users);

            return BadRequest(new {Message = "No Users"});
        }

        [HttpGet("GetUserByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userServices.GetUserByUsernameAsync(username);

            if (user != null) return Ok(user);

            return BadRequest(new {Message = "No User Found"});
        }

        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userServices.GetUserByEmailAsync(email);

            if (user != null) return Ok(user);

            return BadRequest(new {Message = "No User Found"});
        }

        [HttpGet("GetUserByUsernameAndEmail/{username}/{email}")]
        public async Task<IActionResult> GetUserByUsernameAndEmail(string username, string email)
        {
            var user = await _userServices.GetUserByUsernameAndEmailAsync(username, email);

            if (user != null) return Ok(user);

            return BadRequest(new {Message = "No User Found"});
        }

        [HttpGet("GetUserByUserId/{userId}")]
        public async Task<IActionResult> GetUserByUserId(int userId)
        {
            var user = await _userServices.GetUserByUserId(userId);

            if (user != null) return Ok(user);

            return BadRequest(new {Message = "No User Found"});
        }

        [HttpPut("RequestFriend/{requestingUserId}/{requestedUserId}")]
        public async Task<IActionResult> RequestFriend(int requestingUserId, int requestedUserId)
        {
            var success = await _userServices.RequestFriend(requestingUserId, requestedUserId);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "User Friends Failed To Update"});
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser([FromBody]UserModel user)
        {
            var success = await _userServices.EditUsersAsync(user);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "User Failed To Update"});
        }

        [HttpPut("FollowUser/{userWhoIsFollowingId}/{userBeingFollowedId}")]
        public async Task<IActionResult> FollowUser(int userWhoIsFollowingId, int userBeingFollowedId)
        {
            var success = await _userServices.FollowUser(userWhoIsFollowingId, userBeingFollowedId);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "User Follow Failed To Update"});
        }
    }
}