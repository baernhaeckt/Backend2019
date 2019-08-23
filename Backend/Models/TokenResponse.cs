using System;

namespace Backend.Controllers
{
    public class TokenResponse
    {
        public UserResponse User { get; set; }

        public Guid Id { get; set; }

        public String Text { get; set; }

        public int Points { get; set; }

        public bool Valid { get; set; }
    }
}