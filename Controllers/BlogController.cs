using Microsoft.AspNetCore.Mvc;
using plato_backend.Model;
using plato_backend.Services;

namespace plato_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogServices _blogServices;

        public BlogController(BlogServices blogServices)
        {
            _blogServices = blogServices;
        }

        [HttpGet("GetAllBlogs")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _blogServices.GetBlogsAsync();

            if (blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs"});
        }

        [HttpPost("AddBlog")]
        public async Task<IActionResult> AddBlog([FromBody]BlogModel blog)
        {
            var success = await _blogServices.AddBlogAsync(blog);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Blog Was not Added"});
        }

        [HttpPut("EditBlog")]
        public async Task<IActionResult> EditBlog([FromBody]BlogModel blog)
        {
            var success = await _blogServices.EditBlogsAsync(blog);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Blog Failed To Update"});
        }

        [HttpDelete("DeleteBlog")]
        public async Task<IActionResult> DeleteBlog([FromBody]BlogModel blog)
        {
            var success = await _blogServices.EditBlogsAsync(blog);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Blog Failed To Update"});
        }

        [HttpGet("GetBlogsByUserId/{userId}")]
        public async Task<IActionResult> GetBlogsByUserId(int userId)
        {
            var blogs = await _blogServices.GetBlogsByUserIdAsync(userId);

            if (blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs"});
        }

        [HttpGet("GetBlogsByDate/{date}")]
        public async Task<IActionResult> GetBlogsByDate(string date)
        {
            var blogs = await _blogServices.GetBlogsByDateAsync(date);

            if (blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs by that Date"});
        }

        [HttpGet("GetBlogsByTags/{tags}")]
        public async Task<IActionResult> GetBlogsByTags(string[] tags)
        {
            var blogs = await _blogServices.GetBlogsByTagsAsync(tags);

            if (blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs with those Tags"});
        }
    }
}