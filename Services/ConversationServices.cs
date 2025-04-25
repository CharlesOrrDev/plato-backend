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

        public async Task<bool> AddConversationAsync(ConversationModel conversation)
        {
            ConversationModel conversationToAdd = new();

            conversationToAdd.Id = conversation.Id;
            conversationToAdd.UserOneId = conversation.UserOneId;
            conversationToAdd.UserTwoId = conversation.UserTwoId;
            conversationToAdd.CreationDate = DateTime.Now.ToString();

            await _dataContext.Conversation.AddAsync(conversationToAdd);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> AddMessageAsync(MessageModel message)
        {
            MessageModel messageToAdd = new();

            messageToAdd.Id = message.Id;
            messageToAdd.ConversationId = message.ConversationId;
            messageToAdd.UserId = message.UserId;
            messageToAdd.Message = message.Message;
            messageToAdd.DateSent = DateTime.Now.ToString();

            await _dataContext.Message.AddAsync(messageToAdd);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<ConversationModel> GetConversationByIdAsync(int conversationId)
        {
            return (await _dataContext.Conversation.FindAsync(conversationId))!;
        }

        public async Task<MessageModel> GetMessageByIdAsync(int messageId)
        {
            return (await _dataContext.Message.FindAsync(messageId))!;
        }

        public async Task<List<ConversationModel>> GetConversationsByUserOneIdAsync(int userOneId)
        {
            return await _dataContext.Conversation.Where(conversation => conversation.UserOneId == userOneId).ToListAsync();
        }

        public async Task<List<ConversationModel>> GetConversationsByUserTwoIdAsync(int userTwoId)
        {
            return await _dataContext.Conversation.Where(conversation => conversation.UserTwoId == userTwoId).ToListAsync();
        }

        public async Task<List<MessageModel>> GetMessagesByUserIdAsync(int userId)
        {
            return await _dataContext.Message.Where(message => message.UserId == userId).ToListAsync();
        }

        public async Task<List<MessageModel>> GetMessagesByUserIdAndConversationIdAsync(int userId, int ConversationId)
        {
            return await _dataContext.Message.Where(message => message.UserId == userId && message.ConversationId == ConversationId).ToListAsync();
        }

        public async Task<bool> EditConversationsAsync(ConversationModel conversation)
        {
            var conversationToEdit = await GetConversationByIdAsync(conversation.Id);

            if (conversationToEdit == null) return false;
            
            conversationToEdit.UserOneId = conversation.UserOneId;
            conversationToEdit.UserTwoId = conversation.UserTwoId;

            _dataContext.Conversation.Update(conversationToEdit);
            
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditMessageAsync(MessageModel message)
        {
            var messageToEdit = await GetMessageByIdAsync(message.Id);

            if (messageToEdit == null) return false;
            
            messageToEdit.ConversationId = message.ConversationId;
            messageToEdit.UserId = message.UserId;
            messageToEdit.Message = message.Message;

            _dataContext.Message.Update(messageToEdit);
            
            return await _dataContext.SaveChangesAsync() != 0;
        }
    }
}