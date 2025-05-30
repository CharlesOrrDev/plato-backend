public class BlogCreateRequest
{
    public int UserId { get; set; }
    public string? PublisherName { get; set; }
    public string? Date { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? RecipeName { get; set; }
    public string? Description { get; set; }
    public List<string>? Tags { get; set; }
    public string? PostType { get; set; }
    public string? TotalTime { get; set; }
    public string? Servings { get; set; }
    public string? Source { get; set; }
    public bool IsPublished { get; set; }
}