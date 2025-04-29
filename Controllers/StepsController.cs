using Microsoft.AspNetCore.Mvc;
using plato_backend.Model;
using plato_backend.Services;

namespace plato_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StepsController : ControllerBase
    {
        private readonly StepsServices _stepsServices;

        public StepsController(StepsServices stepsServices)
        {
            _stepsServices = stepsServices;
        }

        [HttpGet("GetAllSteps")]
        public async Task<IActionResult> GetAllSteps()
        {
            var steps = await _stepsServices.GetStepsAsync();

            if (steps != null) return Ok(steps);

            return BadRequest(new {Message = "No Steps"});
        }

        [HttpPost("AddSteps")]
        public async Task<IActionResult> AddSteps([FromBody]StepsModel steps)
        {
            var success = await _stepsServices.AddStepsAsync(steps);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Steps Was not Added"});
        }

        [HttpPut("EditSteps")]
        public async Task<IActionResult> EditSteps([FromBody]StepsModel steps)
        {
            var success = await _stepsServices.EditStepsAsync(steps);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Steps Failed To Update"});
        }


        [HttpGet("GetStepsById/{id}")]
        public async Task<IActionResult> GetStepsById(int id)
        {
            var steps = await _stepsServices.GetStepsByIdAsync(id);

            if (steps != null) return Ok(steps);

            return BadRequest(new {Message = "No Steps"});
        }

        [HttpGet("GetStepsByBlogId/{blogId}")]
        public async Task<IActionResult> GetStepsByBlogId(int blogId)
        {
            var steps = await _stepsServices.GetStepsByBlogIdAsync(blogId);

            if (steps != null) return Ok(steps);

            return BadRequest(new {Message = "No Steps"});
        }
    }
}