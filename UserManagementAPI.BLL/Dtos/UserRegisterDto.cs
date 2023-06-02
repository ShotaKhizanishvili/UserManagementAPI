using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.BLL.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal Number can only be 11 characters long")]
        public string PersonalNumber { get; set; } = string.Empty;

    }
}
