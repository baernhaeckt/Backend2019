using System;

namespace Backend.Core.Features.UserManagement.Commands
{
    public class RefreshTokenCommandResult
    {
        public RefreshTokenCommandResult(bool tokenNotFound, bool isExpired, bool isAlreadyUsed, bool isRevoked, Guid userId, string? token = null)
        {
            TokenNotFound = tokenNotFound;
            IsExpired = isExpired;
            IsAlreadyUsed = isAlreadyUsed;
            IsRevoked = isRevoked;
            UserId = userId;
            Token = token;
        }

        public bool TokenNotFound { get; }

        public bool IsExpired { get; }

        public bool IsAlreadyUsed { get; }

        public bool IsRevoked { get; }

        public string? Token { get; }

        public string UserId { get; }
    }
}