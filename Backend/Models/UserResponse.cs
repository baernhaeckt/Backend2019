using System;

namespace Backend.Controllers
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public int Points { get; set; }

        public LocationResponse Location { get; set; }

    }
}