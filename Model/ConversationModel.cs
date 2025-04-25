namespace plato_backend.Model
{
    public class ConversationModel
    {
        public int Id { get; set; }

        public int UserOneId { get; set; }

        public int UserTwoId { get; set; }

        public string? CreationDate { get; set; }
    }
}