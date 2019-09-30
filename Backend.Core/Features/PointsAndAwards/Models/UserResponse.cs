using System;

namespace Backend.Core.Features.PointsAndAwards.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public int Points { get; set; }
    }
}