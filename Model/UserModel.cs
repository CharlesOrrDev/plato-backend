namespace plato_backend.Model
{
    public class UserModel
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? Salt { get; set; }

        public string? Hash { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? DateOfBirth { get; set; }

        public string? ProfilePicture { get; set; }

        public List<int>? LikedBlogs { get; set; }

        public List<int>? RatedBlogs { get; set; }

        public string? DateCreated { get; set; }

        public List<int>? IncomingFriendRequest { get; set; }

        public List<int>? OutgoingFriendRequest { get; set; }

        public List<int>? Friends { get; set; }

        public bool PremiumMember { get; set; }

        public List<string>? Interests { get; set; }

        public List<int>? SavedRecipes { get; set; }

        public List<int>? Following { get; set; }

        public List<int>? Followers { get; set; }
    }
}