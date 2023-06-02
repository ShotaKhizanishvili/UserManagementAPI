using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementAPI.DAL.Data
{
    public class UserProfile
    {
        [ForeignKey("User")]
        public int Id { get; set; }

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal Number can only be 11 characters long")]
        public string PersonalNumber { get; set; } = string.Empty;

        public virtual User User { get; set; }
    }
}
