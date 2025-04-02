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
            if (await DoesUserExist(newUser.Username!, newUser.Email!)) return false;

            UserModel userToAdd = new();

            PasswordDTO encryptedPassword = HashPassword(newUser.Password!);

            userToAdd.Hash = encryptedPassword.Hash;
            userToAdd.Salt = encryptedPassword.Salt;
            userToAdd.Email = newUser.Email;
            userToAdd.Username = newUser.Username;
            userToAdd.Name = newUser.Name;
            userToAdd.PhoneNumber = newUser.PhoneNumber;
            userToAdd.DateOfBirth = newUser.DateOfBirth;

            await _dataContext.User.AddAsync(userToAdd);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        private async Task<bool> DoesUserExist(string email, string username)
        {
            return await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username || user.Email == email) != null;
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
                // issuer: "https://platobackend-a7hagaahdvdfesgm.westus-01.azurewebsites.net",
                issuer: "https://localhost:5000",
                // audience: "https://platobackend-a7hagaahdvdfesgm.westus-01.azurewebsites.net",
                audience: "https://localhost:5000",
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

            return user;
        }
    }
}