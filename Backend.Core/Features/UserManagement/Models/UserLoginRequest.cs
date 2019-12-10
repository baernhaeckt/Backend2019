using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.UserManagement.Models
{
    public class UserLoginRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }
    }
}