using System;

namespace Backend.Core.Features.Friendship.Models
{
    public class FriendResponse
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public int Points { get; set; }

        public double Co2Saving { get; set; }

        public string Email { get; set; } = string.Empty;

        public Location? Location { get; set; }
    }
}