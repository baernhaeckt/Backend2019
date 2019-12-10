using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.Friendship.Models
{
    public class AddFriendRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }
    }
}