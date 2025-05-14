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

        public virtual List<IngredientsModel>? Ingredients { get; set; }

        public virtual List<StepsModel>? Steps { get; set; }

        public List<string>? Tags { get; set; }

        public int Rating { get; set; }

        public int NumberOfRatings { get; set; }

        public int AverageRating { get; set; }

        public int NumberOfLikes { get; set; }

        public string? PostType { get; set; }

        public string? TotalTime { get; set; }

        public string? Servings { get; set; }

        public string? Source { get; set; }

        public bool IsPublished { get; set; }

        public bool IsDeleted { get; set; }
    }
}