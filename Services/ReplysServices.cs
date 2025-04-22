using Microsoft.EntityFrameworkCore;
using plato_backend.Context;
using plato_backend.Model;

namespace plato_backend.Services
{
    public class ReplysServices
    {
        private readonly DataContext _dataContext;

        public ReplysServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ReplysModel>> GetReplyAsync()
        {
            return await _dataContext.Reply.ToListAsync();
        }

        public async Task<bool> AddReplyAsync(ReplysModel reply)
        {
            await _dataContext.Reply.AddAsync(reply);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditReplyAsync(ReplysModel reply)
        {
            var replyToEdit = await GetReplyByIdAsync(reply.Id);

            if (replyToEdit == null) return false;
            
            replyToEdit.UserId = reply.UserId;
            replyToEdit.PublisherName = reply.PublisherName;
            replyToEdit.Date = reply.Date;
            replyToEdit.Reply = reply.Reply;
            replyToEdit.IsPublished = reply.IsPublished;
            replyToEdit.IsDeleted = reply.IsDeleted;

            _dataContext.Reply.Update(replyToEdit);
            
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<ReplysModel> GetReplyByIdAsync(int id)
        {
            return (await _dataContext.Reply.FindAsync(id))!;
        }

        public async Task<List<ReplysModel>> GetReplyByUserIdAsync(int id)
        {
            return await _dataContext.Reply.Where(reply => reply.UserId == id).ToListAsync();
        }

        public async Task<List<ReplysModel>> GetReplyByDateAsync(string date)
        {
            return await _dataContext.Reply.Where(reply => reply.Date == date).ToListAsync();
        }

        public async Task<List<ReplysModel>> GetReplyByCommentIdAsync(int id)
        {
            return await _dataContext.Reply.Where(reply => reply.CommentId == id).ToListAsync();
        }
    }
}