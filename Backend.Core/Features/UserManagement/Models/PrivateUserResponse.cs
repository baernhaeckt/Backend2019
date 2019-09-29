using System;

namespace Backend.Core.Features.UserManagement.Models
{
    public class PrivateUserResponse
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public int Points { get; set; }

        public string Email { get; set; }

        public LocationResponse Location { get; set; }
    }
}