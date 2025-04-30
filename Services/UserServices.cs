using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using plato_backend.Context;
using plato_backend.Model;
using plato_backend.Model.DTOS;

namespace plato_backend.Services
{
    public class UserServices
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;

        public UserServices(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }

        public async Task<bool> CreateAccount(UserDTO newUser)
        {
            if (await DoesUserExistUsername(newUser.Username!) || (await DoesUserExistEmail(newUser.Email!))) return false;

            UserModel userToAdd = new();

            PasswordDTO encryptedPassword = HashPassword(newUser.Password!);

            userToAdd.Hash = encryptedPassword.Hash;
            userToAdd.Salt = encryptedPassword.Salt;
            userToAdd.Email = newUser.Email;
            userToAdd.Username = newUser.Username;
            userToAdd.Name = newUser.Name;
            userToAdd.PhoneNumber = newUser.PhoneNumber;
            userToAdd.DateOfBirth = newUser.DateOfBirth;
            userToAdd.ProfilePicture = "";
            userToAdd.LikedBlogs = [];
            userToAdd.RatedBlogs = [];
            userToAdd.DateCreated = DateTime.Now.ToString();
            userToAdd.IncomingFriendRequest = [];
            userToAdd.OutgoingFriendRequest = [];
            userToAdd.Friends = [];
            userToAdd.PremiumMember = false;
            userToAdd.Interests = [];
            userToAdd.SavedRecipes = [];

            await _dataContext.User.AddAsync(userToAdd);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            return await _dataContext.User.ToListAsync();
        }

        private async Task<bool> DoesUserExistUsername(string username)
        {
            return await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username) != null;
        }

        private async Task<bool> DoesUserExistEmail(string email)
        {
            return await _dataContext.User.SingleOrDefaultAsync(user => user.Email == email) != null;
        }

        private static PasswordDTO HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(saltBytes);

            string hash;
            
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }

            return new PasswordDTO
            {
                Salt = salt,
                Hash = hash
            };
        }

        public async Task<string> Login(UserLoginDTO user)
        {
            UserModel currentUser = await GetUserByUsernameOrEmail(user.Username!, user.Email!);

            if (currentUser == null) return null!;

            if (!VerifyPassword(user.Password!, currentUser.Salt!, currentUser.Hash!)) return null!;
            
            return GenerateJWToken([]);
        }

        private string GenerateJWToken(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken
            (
                issuer: "https://platobackend-a7hagaahdvdfesgm.westus-01.azurewebsites.net",
                // issuer: "https://localhost:5000",
                audience: "https://platobackend-a7hagaahdvdfesgm.westus-01.azurewebsites.net",
                // audience: "https://localhost:5000",
                claims: claims,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private async Task<UserModel> GetUserByUsernameOrEmail(string username, string email)
        {
            return (await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username || user.Email == email))!;
        }

        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltByte = Convert.FromBase64String(salt);

            string checkHash;

            using(var deriveBytes = new Rfc2898DeriveBytes(password, saltByte, 310000, HashAlgorithmName.SHA256))
            {
                checkHash = Convert.ToBase64String(deriveBytes.GetBytes(32));
                return hash == checkHash;
            }
        }

        public async Task<UserInfoDTO> GetUserByUsernameAsync(string username)
        {
            var currentUser = await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username);

            UserInfoDTO user = new();

            user.Id = currentUser!.Id;
            user.Email = currentUser.Email;
            user.Username = currentUser.Username;
            user.Name = currentUser.Name;
            user.PhoneNumber = currentUser.PhoneNumber;
            user.DateOfBirth = currentUser.DateOfBirth;
            user.ProfilePicture = currentUser.ProfilePicture;
            user.LikedBlogs = currentUser.LikedBlogs;
            user.RatedBlogs = currentUser.RatedBlogs;
            user.DateCreated = currentUser.DateCreated;
            user.IncomingFriendRequest = currentUser.IncomingFriendRequest;
            user.OutgoingFriendRequest = currentUser.OutgoingFriendRequest;
            user.Friends = currentUser.Friends;
            user.PremiumMember = currentUser.PremiumMember;
            user.Interests = currentUser.Interests;
            user.SavedRecipes = currentUser.SavedRecipes;

            return user;
        }

        public async Task<UserInfoDTO> GetUserByEmailAsync(string email)
        {
            var currentUser = await _dataContext.User.SingleOrDefaultAsync(user => user.Email == email);

            UserInfoDTO user = new();

            user.Id = currentUser!.Id;
            user.Email = currentUser.Email;
            user.Username = currentUser.Username;
            user.Name = currentUser.Name;
            user.PhoneNumber = currentUser.PhoneNumber;
            user.DateOfBirth = currentUser.DateOfBirth;
            user.ProfilePicture = currentUser.ProfilePicture;
            user.LikedBlogs = currentUser.LikedBlogs;
            user.RatedBlogs = currentUser.RatedBlogs;
            user.DateCreated = currentUser.DateCreated;
            user.IncomingFriendRequest = currentUser.IncomingFriendRequest;
            user.OutgoingFriendRequest = currentUser.OutgoingFriendRequest;
            user.Friends = currentUser.Friends;
            user.PremiumMember = currentUser.PremiumMember;
            user.Interests = currentUser.Interests;
            user.SavedRecipes = currentUser.SavedRecipes;

            return user;
        }

        public async Task<UserInfoDTO> GetUserByUsernameAndEmailAsync(string username, string email)
        {
            var currentUser = await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username && user.Email == email);

            UserInfoDTO user = new();

            user.Id = currentUser!.Id;
            user.Email = currentUser.Email;
            user.Username = currentUser.Username;
            user.Name = currentUser.Name;
            user.PhoneNumber = currentUser.PhoneNumber;
            user.DateOfBirth = currentUser.DateOfBirth;
            user.ProfilePicture = currentUser.ProfilePicture;
            user.LikedBlogs = currentUser.LikedBlogs;
            user.RatedBlogs = currentUser.RatedBlogs;
            user.DateCreated = currentUser.DateCreated;
            user.IncomingFriendRequest = currentUser.IncomingFriendRequest;
            user.OutgoingFriendRequest = currentUser.OutgoingFriendRequest;
            user.Friends = currentUser.Friends;
            user.PremiumMember = currentUser.PremiumMember;
            user.Interests = currentUser.Interests;
            user.SavedRecipes = currentUser.SavedRecipes;

            return user;
        }

        public async Task<UserModel> GetUserByUserId(int userId)
        {
            return (await _dataContext.User.FindAsync(userId))!;
        }

        public async Task<bool> RequestFriend(int senderUserId, int receiverUserId)
        {
            var senderUser = await GetUserByUserId(senderUserId);

            var receiverUser = await GetUserByUserId(receiverUserId);

            if (senderUser.Friends!.Contains(receiverUserId))
            {
                senderUser.Friends.Remove(receiverUserId);
                receiverUser.Friends!.Remove(senderUserId);
            }else if (senderUser.IncomingFriendRequest!.Contains(receiverUserId))
            {
                receiverUser.OutgoingFriendRequest!.Remove(senderUserId);
                senderUser.IncomingFriendRequest.Remove(receiverUserId);
                senderUser.Friends.Add(receiverUserId);
                receiverUser.Friends!.Add(senderUserId);
            }else if (!receiverUser.IncomingFriendRequest!.Contains(senderUserId))
            {
                receiverUser.IncomingFriendRequest.Add(senderUserId);
                senderUser.OutgoingFriendRequest!.Add(receiverUserId);
            }else if (senderUser.OutgoingFriendRequest!.Contains(receiverUserId))
            {
                receiverUser.IncomingFriendRequest.Remove(senderUserId);
                senderUser.OutgoingFriendRequest.Remove(receiverUserId);
            }

            _dataContext.User.Update(senderUser);
            _dataContext.User.Update(receiverUser);

            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditUsersAsync(UserModel user)
        {
            var userToEdit = await GetUserByUserId(user.Id);

            if (userToEdit == null) return false;
            
            userToEdit.Id = user.Id;
            userToEdit.Email = user.Email;
            userToEdit.Username = user.Username;
            userToEdit.Salt = user.Salt;
            userToEdit.Hash = user.Hash;
            userToEdit.Name = user.Name;
            userToEdit.PhoneNumber = user.PhoneNumber;
            userToEdit.DateOfBirth = user.DateOfBirth;
            userToEdit.ProfilePicture = user.ProfilePicture;
            userToEdit.LikedBlogs = user.LikedBlogs;
            userToEdit.RatedBlogs = user.RatedBlogs;
            userToEdit.DateCreated = user.DateCreated;
            userToEdit.IncomingFriendRequest = user.IncomingFriendRequest;
            userToEdit.OutgoingFriendRequest = user.OutgoingFriendRequest;
            userToEdit.Friends = user.Friends;
            userToEdit.PremiumMember = user.PremiumMember;
            userToEdit.Interests = user.Interests;
            userToEdit.SavedRecipes = user.SavedRecipes;

            _dataContext.User.Update(userToEdit);
            
            return await _dataContext.SaveChangesAsync() != 0;
        }
    }
}