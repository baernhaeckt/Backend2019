using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.UserManagement.Shared.Abstraction;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;

namespace Backend.Core.Features.UserManagement.Shared
{
    internal class RefreshTokenStorage : IRefreshTokenStorage
    {
        private const int RefreshTokenExpirationInDays = 2;

        private readonly IClock _clock;

        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenStorage(IUnitOfWork unitOfWork, IClock clock)
        {
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public async Task<string> Create(Guid userId)
        {
            byte[] newRefreshToken = CreateTokenValue(out string newBase64RefreshTokenHash);

            await _unitOfWork.InsertAsync(new RefreshToken { ExpiresAt = _clock.Now().AddDays(RefreshTokenExpirationInDays), UserId = userId, ValueHash = newBase64RefreshTokenHash });

            return Convert.ToBase64String(newRefreshToken);
        }

        public async Task<RefreshToken?> Retrieve(string tokenValue)
        {
            string refreshTokenBase64Hash = TokenHashFromTokenValue(tokenValue);
            var currentRefreshToken = await _unitOfWork.SingleOrDefaultAsync<RefreshToken>(t => t.ValueHash == refreshTokenBase64Hash);
            return currentRefreshToken;
        }

        private static byte[] CreateTokenValue(out string newBase64RefreshTokenHash)
        {
            var newRefreshToken = new byte[256];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(newRefreshToken);
            }

            using var sha256 = SHA256.Create();
            byte[] newRefreshTokenHash = sha256.ComputeHash(newRefreshToken);
            newBase64RefreshTokenHash = Convert.ToBase64String(newRefreshTokenHash);
            return newRefreshToken;
        }

        private static string TokenHashFromTokenValue(string tokenValue)
        {
            byte[] refreshToken = Convert.FromBase64String(tokenValue);
            using var sha256Hash = SHA256.Create();
            byte[] refreshTokenRawHash = sha256Hash.ComputeHash(refreshToken);
            string refreshTokenBase64Hash = Convert.ToBase64String(refreshTokenRawHash, Base64FormattingOptions.None);
            return refreshTokenBase64Hash;
        }
    }
}