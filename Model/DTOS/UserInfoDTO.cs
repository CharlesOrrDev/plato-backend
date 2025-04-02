namespace plato_backend.Model.DTOS
{
    public class UserInfoDTO
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? DateOfBirth { get; set; }

        public string? ProfilePicture { get; set; }
    }
}