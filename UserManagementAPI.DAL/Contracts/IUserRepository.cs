using UserManagementAPI.BLL.Dtos;
using UserManagementAPI.BLL.JsonModels;
using UserManagementAPI.DAL.Data;

namespace UserManagementAPI.DAL.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public string GetMyName();
        public Task<User> Register(UserRegisterDto request);
        public Task<string> Login(UserLoginDto request);
        public List<UserPosts> GetPostsByUserId(int id);
        public List<PostsComments> GetCommentsByUserId(int id);
        public List<AlbumsPhotos> GetAlbumsPhotosByUserId(int id);
        public List<UserTodos> GetTodosByUserId(int id);
    }
}
