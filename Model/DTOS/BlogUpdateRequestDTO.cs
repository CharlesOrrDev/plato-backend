public class BlogUpdateRequest : BlogCreateRequest
{
    public IFormFile? NewImageFile { get; set; }
}