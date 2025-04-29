namespace plato_backend.Model
{
    public class StepsModel
    {
        public int Id { get; set; }

        public int BlogId { get; set; }

        public string? Title { get; set; }

        public string[]? Steps { get; set; }
    }
}