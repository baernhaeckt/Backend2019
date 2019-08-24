using System;

namespace Backend.Web.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public int Points { get; set; }
    }
}
