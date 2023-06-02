using UserManagementAPI.BLL.Dtos;
using UserManagementAPI.DAL.Data;
using UserManagementAPI.DAL.Repository;

namespace UserManagementAPI.DAL.Contracts
{
    public interface IUserProfileRepository : IGenericRepository<UserProfile>
    {
        public Task AddRegisterInfo(UserRegisterDto request, int id);
        public Task<UserProfile> UpdateUserProfileAsync(int id, UserProfileDto dto);
    }
}
