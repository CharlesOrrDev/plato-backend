namespace plato_backend.Model
{
    public class MessageModel
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }

        public int UserId { get; set; }

        public string? Message { get; set; }

        public string? DateSent { get; set; }
    }
}