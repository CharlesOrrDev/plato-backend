using Microsoft.EntityFrameworkCore;
using plato_backend.Context;
using plato_backend.Model;

namespace plato_backend.Services
{
    public class BlogServices
    {
        private readonly DataContext _dataContext;
        private readonly BlobStorageService _blobStorageService;

        public BlogServices(DataContext dataContext, BlobStorageService blobStorageService)
        {
            _dataContext = dataContext;
            _blobStorageService = blobStorageService;
        }

        public async Task<string> UploadBlogImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("No image file provided");
            }

            var fileName = $"blog_{Guid.NewGuid()}_{imageFile.FileName}";

            return await _blobStorageService.UploadImageAsync(imageFile, fileName);
        }

        public async Task<List<BlogModel>> GetBlogsAsync()
        {
            return await _dataContext.Blog.Include(blog => blog.Ingredients).Include(blog => blog.Steps).ToListAsync();
        }

        public async Task<bool> AddBlogAsync(BlogModel blog, IFormFile imageFile = null!)
        {
            if (imageFile != null)
            {
                blog.Image = await UploadBlogImageAsync(imageFile);
            }

            await _dataContext.Blog.AddAsync(blog);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<List<IngredientsModel>> GetIngredientsByBlogIdAsync(int blogId)
        {
            return await _dataContext.Ingredients.Where(ingredients => ingredients.BlogId == blogId).ToListAsync();
        }

        public async Task<List<StepsModel>> GetStepsByBlogIdAsync(int blogId)
        {
            return await _dataContext.Steps.Where(steps => steps.BlogId == blogId).ToListAsync();
        }

        public async Task<bool> EditBlogsAsync(BlogModel blog, IFormFile newImageFile = null!)
        {
            var blogToEdit = await GetBlogByIdAsync(blog.Id);
            if (blogToEdit == null) return false;

            if (newImageFile != null)
            {
                if (!string.IsNullOrEmpty(blogToEdit.Image))
                {
                    await _blobStorageService.DeleteImageAsync(blogToEdit.Image);
                }

                blog.Image = await UploadBlogImageAsync(newImageFile);
            }
            
            blogToEdit.UserId = blog.UserId;
            blogToEdit.PublisherName = blog.PublisherName;
            blogToEdit.Date = blog.Date;
            blogToEdit.Image = blog.Image;
            blogToEdit.RecipeName = blog.RecipeName;
            blogToEdit.Description = blog.Description;
            blogToEdit.Ingredients = blog.Ingredients;
            blogToEdit.Steps = blog.Steps;
            blogToEdit.Tags = blog.Tags;
            blogToEdit.Rating = blog.Rating;
            blogToEdit.NumberOfRatings = blog.NumberOfRatings;
            blogToEdit.AverageRating = blog.AverageRating;
            blogToEdit.PostType = blog.PostType;
            blogToEdit.TotalTime = blog.TotalTime;
            blogToEdit.Servings = blog.Servings;
            blogToEdit.Source = blog.Source;
            blogToEdit.IsPublished = blog.IsPublished;
            blogToEdit.IsDeleted = blog.IsDeleted;

            _dataContext.Blog.Update(blogToEdit);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteBlogAsync(int blogId)
        {
            var blog = await GetBlogByIdAsync(blogId);
            if (blog == null) return false;

            if (!string.IsNullOrEmpty(blog.Image))
            {
                await _blobStorageService.DeleteImageAsync(blog.Image);
            }

            blog.IsDeleted = true;
            _dataContext.Blog.Update(blog);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<List<BlogModel>> getBlogsAsync()
        {
            return await _dataContext.Blog.Include(blog => blog.Ingredients).Include(blog => blog.Steps).ToListAsync();
        }

        public async Task<BlogModel> GetBlogByIdAsync(int id)
        {
            return (await _dataContext.Blog.Include(blog => blog.Ingredients).Include(blog => blog.Steps).FirstOrDefaultAsync(blog => blog.Id == id))!;
        }

        public async Task<List<BlogModel>> GetBlogsByUserIdAsync(int userId)
        {
            return await _dataContext.Blog.Include(blog => blog.Ingredients).Include(blog => blog.Steps).Where(blog => blog.UserId == userId).ToListAsync();
        }

        public async Task<List<BlogModel>> GetBlogsByDateAsync(string date)
        {
            return await _dataContext.Blog.Include(blog => blog.Ingredients).Include(blog => blog.Steps).Where(blog => blog.Date == date).ToListAsync();
        }

        public async Task<List<BlogModel>> GetBlogsByTagsAsync(string tags)
        {
            string[] tagsArray = tags.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();

            return await _dataContext.Blog.Include(blog => blog.Ingredients).Include(blog => blog.Steps).Where(blog => blog.Tags != null && tagsArray.Any(tag => blog.Tags.Contains(tag))).ToListAsync();
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