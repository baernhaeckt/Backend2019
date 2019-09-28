using System;

namespace Backend.Models
{
    public class TokenResponse
    {
        public PrivateUserResponse User { get; set; }

        public Guid Id { get; set; }

        public string Text { get; set; }

        public int Points { get; set; }

        public bool Valid { get; set; }
    }
}