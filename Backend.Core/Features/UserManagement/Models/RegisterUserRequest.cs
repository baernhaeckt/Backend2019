using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.UserManagement.Models
{
    public class RegisterUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
    }
}