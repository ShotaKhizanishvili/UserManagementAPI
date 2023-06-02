using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementAPI.BLL.Dtos
{
    public class UserProfileDto
    {
        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal Number can only be 11 characters long")]
        public string PersonalNumber { get; set; } = string.Empty;
    }
}
