using System;

namespace Backend.Core.Features.UserManagement.Models
{
    public class PrivateUserResponse
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public int Points { get; set; }

        public string Email { get; set; } = string.Empty;

        public LocationResponse? Location { get; set; }
    }
}