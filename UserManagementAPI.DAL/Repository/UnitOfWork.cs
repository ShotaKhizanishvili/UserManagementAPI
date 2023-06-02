using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UserManagementAPI.DAL.Contracts;
using UserManagementAPI.DAL.Data;

namespace UserManagementAPI.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserManagementDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private IUserRepository _userRepository;
        private IUserProfileRepository _userProfileRepository;

        public UnitOfWork(UserManagementDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context, _httpContextAccessor, _configuration);
                }
                return _userRepository;
            }
        }

        public IUserProfileRepository UserProfileRepository
        {
            get
            {
                if (_userProfileRepository == null)
                {
                    _userProfileRepository = new UserProfileRepository(_context,_httpContextAccessor);
                }
                return _userProfileRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
