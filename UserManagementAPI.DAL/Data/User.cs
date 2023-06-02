using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DAL.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
