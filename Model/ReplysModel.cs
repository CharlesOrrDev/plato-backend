namespace plato_backend.Model
{
    public class ReplysModel
    {
        public int Id { get; set; }

        public int CommentId { get; set; }

        public int UserId { get; set; }

        public string? PublisherName { get; set; }

        public string? Date { get; set; }

        public string? Reply { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }
    }
}