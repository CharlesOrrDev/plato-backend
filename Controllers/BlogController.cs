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
        public async Task<IActionResult> AddBlog([FromForm] BlogCreateRequest request)
        {
            var blog = new BlogModel
            {
                UserId = request.UserId,
                PublisherName = request.PublisherName,
                Date = request.Date,
                RecipeName = request.RecipeName,
                Description = request.Description,
                Tags = request.Tags,
                PostType = request.PostType,
                TotalTime = request.TotalTime,
                Servings = request.Servings,
                Source = request.Source,
                IsPublished = request.IsPublished,
                IsDeleted = false
            };

            var success = await _blogServices.AddBlogAsync(blog, request.ImageFile!);

            if (success) return Ok(new { Success = true, ImageUrl = blog.Image });
            return BadRequest(new { Message = "Blog was not added" });
        }

        [HttpPut("EditBlog/{id}")]
        public async Task<IActionResult> EditBlog(int id, [FromForm] BlogUpdateRequest request)
        {
            var blog = new BlogModel
            {
                Id = id,
                UserId = request.UserId,
                PublisherName = request.PublisherName,
                Date = request.Date,
                RecipeName = request.RecipeName,
                Description = request.Description,
                Tags = request.Tags,
                PostType = request.PostType,
                TotalTime = request.TotalTime,
                Servings = request.Servings,
                Source = request.Source,
                IsPublished = request.IsPublished
            };

            var success = await _blogServices.EditBlogsAsync(blog, request.NewImageFile!);

            if (success) return Ok(new { Success = true });
            return BadRequest(new { Message = "Blog failed to update" });
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest(new { Message = "No image file provided" });
            }

            try
            {
                var imageUrl = await _blogServices.UploadBlogImageAsync(imageFile);
                return Ok(new { Success = true, ImageUrl = imageUrl });
            }catch (Exception ex)
            {
                return BadRequest(new { Message = $"Image upload failed: {ex.Message}" });
            }
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

        [HttpGet("GetBlogById/{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            var blog = await _blogServices.GetBlogByIdAsync(id);

            if (blog != null) return Ok(blog);

            return BadRequest(new {Message = "No Blogs"});
        }

        [HttpGet("GetBlogsByDate/{date}")]
        public async Task<IActionResult> GetBlogsByDate(string date)
        {
            var blogs = await _blogServices.GetBlogsByDateAsync(date);

            if (blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs with that Date"});
        }

        [HttpGet("GetBlogsByTags/{tags}")]
        public async Task<IActionResult> GetBlogsByTags(string tags)
        {
            var blogs = await _blogServices.GetBlogsByTagsAsync(tags);

            if (blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs with those Tags"});
        }

        [HttpPut("Rating/{blogId}/{userId}/{Rating}")]
        public async Task<IActionResult> Rating(int blogId, int userId, int Rating)
        {
            var success = await _blogServices.Rating(blogId, userId, Rating);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Blog Rating Failed To Update"});
        }

        [HttpPut("Likes/{blogId}/{userId}")]
        public async Task<IActionResult> Likes(int blogId, int userId)
        {
            var success = await _blogServices.Likes(blogId, userId);

            if (success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Blog Likes Failed To Update"});
        }
    }
}