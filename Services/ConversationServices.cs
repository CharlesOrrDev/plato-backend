using Microsoft.EntityFrameworkCore;
using plato_backend.Context;
using plato_backend.Model;

namespace plato_backend.Services
{
    public class ConversationServices
    {
        private readonly DataContext _dataContext;

        public ConversationServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ConversationModel>> GetAllConversationsAsync()
        {
            return await _dataContext.Conversation.ToListAsync();
        }

        public async Task<ConversationModel> GetConversationById(int conversationId)
        {
            return (await _dataContext.Conversation.FindAsync(conversationId))!;
        }
    }
}