using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.BLL.Dtos
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }
}
