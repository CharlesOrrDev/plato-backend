using Microsoft.AspNetCore.Mvc;
using plato_backend.Model;
using plato_backend.Services;

namespace plato_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IngredientsServices _ingredientsServices;

        public IngredientsController(IngredientsServices stepsServices)
        {
            _ingredientsServices = stepsServices;
        }

        [HttpGet("GetAllIngredients")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientsServices.GetIngredientsAsync();

            if (ingredients != null) return Ok(ingredients);

            return BadRequest(new {Message = "No Ingredients"});
        }

        [HttpPost("AddIngredients")]
        public async Task<IActionResult> AddIngredients([FromBody]IngredientsModel ingredients)
        {
            var success = await _ingredientsServices.AddIngredientsAsync(ingredients);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Ingredients Was not Added"});
        }

        [HttpPut("EditIngredients")]
        public async Task<IActionResult> EditIngredients([FromBody]IngredientsModel ingredients)
        {
            var success = await _ingredientsServices.EditIngredientsAsync(ingredients);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Ingredients Failed To Update"});
        }

        [HttpGet("GetIngredientsById/{id}")]
        public async Task<IActionResult> GetIngredientsById(int id)
        {
            var ingredients = await _ingredientsServices.GetIngredientsByIdAsync(id);

            if (ingredients != null) return Ok(ingredients);

            return BadRequest(new {Message = "No Ingredients"});
        }

        [HttpGet("GetIngredientsByBlogId/{blogId}")]
        public async Task<IActionResult> GetIngredientsByBlogId(int blogId)
        {
            var ingredients = await _ingredientsServices.GetIngredientsByBlogIdAsync(blogId);

            if (ingredients != null) return Ok(ingredients);

            return BadRequest(new {Message = "No Ingredients"});
        }
    }
}