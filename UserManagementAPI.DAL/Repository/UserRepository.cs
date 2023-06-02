using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UserManagementAPI.BLL.Dtos;
using UserManagementAPI.BLL.JsonModels;
using UserManagementAPI.DAL.Contracts;
using UserManagementAPI.DAL.Data;

namespace UserManagementAPI.DAL.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly UserManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private const string BaseUrl = "https://jsonplaceholder.typicode.com/";

        public UserRepository(UserManagementDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            }
            return result;
        }
        public async Task<User> Register(UserRegisterDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true
            };
            
            await AddAsync(user);           

            return user;
        }

        public async Task<string> Login(UserLoginDto request)
        {
            var entity = await _context.Users.SingleAsync(a => a.Username == request.Username);

            if (entity == null)
            {
                return "User not found.";
            }

            if (!VerifyPasswordHash(request.Password, entity.PasswordHash, entity.PasswordSalt))
            {
                return "wrong password.";
            }

            string token = CreateToken(entity);

            return token;
        }

        public List<UserPosts> GetPostsByUserId(int id)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest($"users/{id}/posts", Method.Get);

            var response = client.Execute(request);

            if (response.Content == "[]")
            {
                throw new Exception(message: "Record not found.");
            }

            var records = JsonConvert.DeserializeObject<List<UserPosts>>(response.Content);

            return records;
        }

        public List<PostsComments> GetCommentsByUserId(int id)
        {
            var posts = GetPostsByUserId(id);

            if (posts == null)
            {
                throw new Exception(message: "Record not found.");
            }

            List<PostsComments> allComments = new List<PostsComments>();
            foreach (var post in posts)
            {
                var client = new RestClient(BaseUrl);
                var request = new RestRequest($"posts/{post.id}/comments", Method.Get);

                var response = client.Execute(request);
                if (response.Content == "[]")
                {
                    throw new Exception(message: "Record not found.");
                }
                var records = JsonConvert.DeserializeObject<List<PostsComments>>(response.Content);
                allComments.AddRange(records);
            }
            return allComments;
        }

        public List<AlbumsPhotos> GetAlbumsPhotosByUserId(int id)
        {
            var albums = GetUserAlbumsByUserId(id);

            if (albums == null)
            {
                throw new Exception(message: "Record not found.");
            }

            List<AlbumsPhotos> allPhotos = new List<AlbumsPhotos>();
            foreach (var album in albums)
            {
                var client = new RestClient(BaseUrl);
                var request = new RestRequest($"albums/{album.id}/photos", Method.Get);

                var response = client.Execute(request);
                if (response.Content == "[]")
                {
                    throw new Exception(message: "Record not found.");
                }
                var records = JsonConvert.DeserializeObject<List<AlbumsPhotos>>(response.Content);
                allPhotos.AddRange(records);
            }
            return allPhotos;
        }

        public List<UserTodos> GetTodosByUserId(int id)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest($"users/{id}/todos", Method.Get);

            var response = client.Execute(request);

            if (response.Content == "[]")
            {
                throw new Exception(message: "Record not found.");
            }

            var records = JsonConvert.DeserializeObject<List<UserTodos>>(response.Content);

            return records;
        }


        #region HelperMethods
        private List<UserAlbums> GetUserAlbumsByUserId(int id)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest($"users/{id}/albums", Method.Get);

            var response = client.Execute(request);

            if (response.Content == "[]")
            {
                throw new Exception(message: "Record not found.");
            }

            var records = JsonConvert.DeserializeObject<List<UserAlbums>>(response.Content);

            return records;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }


        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        #endregion
    }
}
