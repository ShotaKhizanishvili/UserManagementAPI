using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Xml.XPath;
using UserManagementAPI.BLL.Dtos;
using UserManagementAPI.DAL.Contracts;
using UserManagementAPI.DAL.Data;

namespace UserManagementAPI.DAL.Repository
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        private readonly UserManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProfileRepository(UserManagementDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddRegisterInfo(UserRegisterDto request, int id)
        {
            var userProfile = new UserProfile
            {
                Id = id,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                PersonalNumber = request.PersonalNumber
            };
            await AddAsync(userProfile);
        }

        public async Task<UserProfile> UpdateUserProfileAsync(int id,UserProfileDto dto)
        {            
            var user = await GetAsync(id);
            if(user == null)
            {
                throw new Exception(message: "Entity not found.");
            }

            user.Firstname = dto.Firstname;
            user.Lastname = dto.Lastname;
            user.PersonalNumber = dto.PersonalNumber;

            await _context.SaveChangesAsync();
            return user;
        }        
    }
}
