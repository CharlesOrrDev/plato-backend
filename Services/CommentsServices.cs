using Microsoft.EntityFrameworkCore;
using plato_backend.Context;
using plato_backend.Model;

namespace plato_backend.Services
{
    public class CommentsServices
    {
        private readonly DataContext _dataContext;

        public CommentsServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<CommentsModel>> GetCommentsAsync()
        {
            return await _dataContext.Comment.ToListAsync();
        }

        public async Task<bool> AddCommentsAsync(CommentsModel comment)
        {
            await _dataContext.Comment.AddAsync(comment);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditCommentsAsync(CommentsModel comment)
        {
            var commentToEdit = await GetCommentsByIdAsync(comment.Id);

            if (commentToEdit == null) return false;
            
            commentToEdit.UserId = comment.UserId;
            commentToEdit.PublisherName = comment.PublisherName;
            commentToEdit.Date = comment.Date;
            commentToEdit.Comment = comment.Comment;
            commentToEdit.IsPublished = comment.IsPublished;
            commentToEdit.IsDeleted = comment.IsDeleted;

            _dataContext.Comment.Update(commentToEdit);
            
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<CommentsModel> GetCommentsByIdAsync(int id)
        {
            return (await _dataContext.Comment.FindAsync(id))!;
        }

        public async Task<List<CommentsModel>> GetCommentsByUserIdAsync(int userId)
        {
            return await _dataContext.Comment.Where(comment => comment.UserId == userId).ToListAsync();
        }

        public async Task<List<CommentsModel>> GetCommentsByDateAsync(string date)
        {
            return await _dataContext.Comment.Where(comment => comment.Date == date).ToListAsync();
        }

        public async Task<List<CommentsModel>> GetCommentsByBlogIdAsync(int blogId)
        {
            return await _dataContext.Comment.Where(comment => comment.BlogId == blogId).ToListAsync();
        }
    }
}