using System;

namespace Backend.Core.Entities
{
    public class RefreshToken : Entity
    {
        public Guid UserId { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }

        public string ValueHash { get; set; } = string.Empty;
    }
}