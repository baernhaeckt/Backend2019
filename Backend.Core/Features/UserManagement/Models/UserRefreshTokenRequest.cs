using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.UserManagement.Models
{
    public class UserRefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}