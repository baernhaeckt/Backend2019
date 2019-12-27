using System;
using System.ComponentModel.DataAnnotations;
using Backend.Core.Framework.Web;

namespace Backend.Core.Features.Friendship.Models
{
    public class RemoveFriendRequest
    {
        [Required]
        [NotEmpty]
        public Guid Id { get; set; }
    }
}