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
            blogToEdit.Tags = blog.Tags;
            blogToEdit.Rating = blog.Rating;
            blogToEdit.NumberOfRatings = blog.NumberOfRatings;
            blogToEdit.AverageRating = blog.AverageRating;
            blogToEdit.PostType = blog.PostType;
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

        public async Task<bool> Rating(int blogId, int userId, int Rating)
        {
            var blogToRate = await GetBlogByIdAsync(blogId);

            var userThatsRating = await GetUserByUserId(userId);

            var rating = Rating;

            if (blogToRate == null) return false;

            if (!userThatsRating.RatedBlogs!.Contains(blogId))
            {
                userThatsRating.RatedBlogs.Add(blogId);
                blogToRate.Rating += rating;
                blogToRate.NumberOfRatings += 1;
                blogToRate.AverageRating = blogToRate.Rating / blogToRate.NumberOfRatings;
            }

            _dataContext.Blog.Update(blogToRate);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<UserModel> GetUserByUserId(int userId)
        {
            return (await _dataContext.User.FindAsync(userId))!;
        }

        public async Task<bool> Likes(int blogId, int userId)
        {
            var blogToLike = await GetBlogByIdAsync(blogId);

            var userThatLikes = await GetUserByUserId(userId);

            if (blogToLike == null) return false;

            if (userThatLikes.LikedBlogs!.Contains(blogId))
            {
                blogToLike.NumberOfLikes -= 1;
            }else
            {
                blogToLike.NumberOfLikes += 1;
            }

            _dataContext.Blog.Update(blogToLike);

            if (!userThatLikes.LikedBlogs!.Contains(blogId))
            {
                userThatLikes.LikedBlogs.Add(blogId);
            }else
            {
                userThatLikes.LikedBlogs.Remove(blogId);
            }

            _dataContext.User.Update(userThatLikes);

            return await _dataContext.SaveChangesAsync() != 0;
        }
    }
}