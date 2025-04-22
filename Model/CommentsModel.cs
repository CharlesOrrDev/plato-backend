namespace plato_backend.Model
{
    public class CommentsModel
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public int UserId { get; set; }

        public string? PublisherName { get; set; }

        public string? Date { get; set; }

        public string? Comment { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }
    }
}