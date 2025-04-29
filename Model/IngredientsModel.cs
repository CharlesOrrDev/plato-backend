namespace plato_backend.Model
{
    public class IngredientsModel
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public string? Title { get; set; }

        public string[]? Ingredients { get; set; }
    }
}