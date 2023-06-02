using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.BLL.Dtos;
using UserManagementAPI.BLL.JsonModels;
using UserManagementAPI.DAL.Contracts;
using UserManagementAPI.DAL.Data;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet, Authorize(Roles = "User")]
        public ActionResult<string> GetMe()
        {
            var userName = _unitOfWork.UserRepository.GetMyName();
            return Ok(userName);
        }


        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
        {
            var result = await _unitOfWork.UserRepository.Register(request);
            await _unitOfWork.UserProfileRepository.AddRegisterInfo(request, result.Id);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            var result = await _unitOfWork.UserRepository.Login(request);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetPostsByUserId")]
        public ActionResult<List<UserPosts>> GetPostsByUserId(int id)
        {
            var result = _unitOfWork.UserRepository.GetPostsByUserId(id);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetCommentsByUserId")]
        public ActionResult<List<PostsComments>> GetCommentsByUserId(int id)
        {
            var result = _unitOfWork.UserRepository.GetCommentsByUserId(id);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetAlbumsPhotosByUserId")]
        public ActionResult<List<AlbumsPhotos>> GetAlbumsPhotosByUserId(int id)
        {
            var result = _unitOfWork.UserRepository.GetAlbumsPhotosByUserId(id);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetTodosByUserId")]
        public ActionResult<List<UserTodos>> GetTodosByUserId(int id)
        {
            var result = _unitOfWork.UserRepository.GetTodosByUserId(id);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateUserProfile")]
        public async Task<ActionResult<UserProfile>> UpdateUserProfile(int id, [FromBody] UserProfileDto user)
        {
            var result = await _unitOfWork.UserProfileRepository.UpdateUserProfileAsync(id, user);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("DeleteUserById")]
        public async Task<ActionResult> DeleteUserById(int id)
        {
            await _unitOfWork.UserRepository.DeleteAsync(id);

            return Ok();
        }

    }
}
