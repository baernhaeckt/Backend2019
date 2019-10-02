using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.UserManagement.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [MaxLength(100)]
        [MinLength(8)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string OldPassword { get; set; } = string.Empty;
    }
}