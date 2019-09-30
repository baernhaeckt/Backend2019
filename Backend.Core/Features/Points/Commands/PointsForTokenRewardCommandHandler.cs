using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Extensions;
using Backend.Infrastructure.Persistence.Abstraction;
using Bogus;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Points.Commands
{
    internal class PointsForTokenRewardCommandHandler : ISubscriber
    {
        private readonly ClaimsPrincipal _claimsPrincipal;

        private readonly PointService _pointService;

        private readonly IUnitOfWork _unitOfWork;

        public PointsForTokenRewardCommandHandler(IUnitOfWork unitOfWork, ClaimsPrincipal claimsPrincipal, PointService pointService)
        {
            _unitOfWork = unitOfWork;
            _claimsPrincipal = claimsPrincipal;
            _pointService = pointService;
        }

        public async Task ExecuteAsync(PointsForTokenRewardCommand command)
        {
            Token token = await _unitOfWork.SingleAsync<Token>(t => t.Value == command.Token);

            if (!token.Valid)
            {
                throw new ValidationException("Token already used.");
            }

            token.UserId = _claimsPrincipal.Id();
            await _unitOfWork.UpdateAsync(token);

            // TODO: Move this logic to this class and decouple using events.
            await _pointService.AddPoints(token);
        }
    }
}