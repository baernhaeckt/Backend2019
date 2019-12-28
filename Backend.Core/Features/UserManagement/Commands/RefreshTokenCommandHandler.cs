using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.UserManagement.Shared.Abstraction;
using Backend.Core.Framework.Cqrs;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement.Commands
{
    internal class RefreshTokenCommandHandler : CommandHandler<RefreshTokenCommand, RefreshTokenCommandResult>
    {
        private readonly IClock _clock;

        private readonly IRefreshTokenStorage _refreshTokenStorage;

        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, ILogger logger, IClock clock, IRefreshTokenStorage refreshTokenStorage)
            : base(unitOfWork, logger)
        {
            _clock = clock;
            _refreshTokenStorage = refreshTokenStorage;
        }

        public override async Task<RefreshTokenCommandResult> ExecuteAsync(RefreshTokenCommand command)
        {
            Logger.ExecuteRefreshToken(command.Token);

            RefreshToken? currentRefreshToken = await _refreshTokenStorage.Retrieve(command.Token);

            if (currentRefreshToken == null)
            {
                Logger.RefreshTokenNotFound(command.Token);
                return new RefreshTokenCommandResult(true, false, false, false);
            }

            if (currentRefreshToken.ExpiresAt < _clock.Now())
            {
                Logger.RefreshTokenExpired(currentRefreshToken.Id, currentRefreshToken.UserId, currentRefreshToken.ValueHash, currentRefreshToken.ExpiresAt);
                return new RefreshTokenCommandResult(false, true, false, false);
            }

            if (currentRefreshToken.IsUsed)
            {
                Logger.RefreshTokenAlreadyUsed(currentRefreshToken.Id, currentRefreshToken.UserId, currentRefreshToken.ValueHash);
                return new RefreshTokenCommandResult(false, false, true, false);
            }

            if (currentRefreshToken.IsRevoked)
            {
                Logger.RefreshTokenRevoked(currentRefreshToken.Id, currentRefreshToken.UserId, currentRefreshToken.ValueHash);
                return new RefreshTokenCommandResult(false, false, false, true);
            }

            // Store new refresh token & expire the old one.
            await UnitOfWork.UpdateAsync<RefreshToken>(currentRefreshToken.Id, new { IsUsed = true });
            string token = await _refreshTokenStorage.Create(currentRefreshToken.UserId);

            Logger.ExecuteRefreshTokenSuccessful(currentRefreshToken.Id, currentRefreshToken.UserId, currentRefreshToken.ValueHash, token);
            return new RefreshTokenCommandResult(false, false, false, false, token);
        }
    }
}