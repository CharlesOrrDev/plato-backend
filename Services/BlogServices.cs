using Microsoft.EntityFrameworkCore;
using plato_backend.Context;
using plato_backend.Model;

namespace plato_backend.Services
{
    public class BlogServices
    {
        private readonly DataContext _dataContext;

        public BlogServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<BlogModel>> GetBlogsAsync()
        {
            return await _dataContext.Blog.ToListAsync();
        }

        public async Task<bool> AddBlogAsync(BlogModel blog)
        {
            await _dataContext.Blog.AddAsync(blog);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditBlogsAsync(BlogModel blog)
        {
            var blogToEdit = await GetBlogByIdAsync(blog.Id);

            if (blogToEdit == null) return false;
            
            blogToEdit.UserId = blog.UserId;
            blogToEdit.PublisherName = blog.PublisherName;
            blogToEdit.Date = blog.Date;
            blogToEdit.Image = blog.Image;
            blogToEdit.RecipeName = blog.RecipeName;
            blogToEdit.Description = blog.Description;
            blogToEdit.Ingredients = blog.Ingredients;
            blogToEdit.Steps = blog.Steps;
            blogToEdit.Tags = blog.Tags;
            blogToEdit.IsPublished = blog.IsPublished;
            blogToEdit.IsDeleted = blog.IsDeleted;

            _dataContext.Blog.Update(blogToEdit);
            
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<BlogModel> GetBlogByIdAsync(int id)
        {
            return (await _dataContext.Blog.FindAsync(id))!;
        }

        public async Task<List<BlogModel>> GetBlogsByUserIdAsync(int id)
        {
            return await _dataContext.Blog.Where(blog => blog.UserId == id).ToListAsync();
        }

        public async Task<List<BlogModel>> GetBlogsByDateAsync(string date)
        {
            return await _dataContext.Blog.Where(blog => blog.Date == date).ToListAsync();
        }

        public async Task<List<BlogModel>> GetBlogsByTagsAsync(string[] tags)
        {
            return await _dataContext.Blog.Where(blog => blog.Tags == tags).ToListAsync();
        }
    }
}