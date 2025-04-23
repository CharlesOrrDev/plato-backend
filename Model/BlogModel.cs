namespace plato_backend.Model
{
    public class BlogModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? PublisherName { get; set; }

        public string? Date { get; set; }

        public string? Image { get; set; }

        public string? RecipeName { get; set; }

        public string? Description { get; set; }

        public string[]? Ingredients { get; set; }

        public string[]? Steps { get; set; }

        public string[]? Tags { get; set; }

        public int Rating { get; set; }

        public int NumberOfRatings { get; set; }

        public int AverageRating { get; set; }

        public int NumberOfLikes { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }
    }
}