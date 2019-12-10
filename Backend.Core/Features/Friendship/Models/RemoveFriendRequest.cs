using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.Friendship.Models
{
    public class RemoveFriendRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}