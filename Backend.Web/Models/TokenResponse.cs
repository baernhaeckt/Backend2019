using System;

namespace Backend.Web.Models
{
    public class TokenResponse
    {
        public PrivateUserResponse User { get; set; }

        public string Id { get; set; }

        public String Text { get; set; }

        public int Points { get; set; }

        public bool Valid { get; set; }
    }
}