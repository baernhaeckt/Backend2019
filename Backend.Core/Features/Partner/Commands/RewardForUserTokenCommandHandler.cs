using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Infrastructure.Persistence.Abstraction;
using Bogus;
using Silverback.Messaging.Publishing;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Partner.Commands
{
    internal class RewardForUserTokenCommandHandler : ISubscriber
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IUnitOfWork _unitOfWork;

        public RewardForUserTokenCommandHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task ExecuteAsync(RewardForUserTokenCommand command)
        {
            Token token = await _unitOfWork.SingleAsync<Token>(t => t.Value == command.TokenId);

            if (!token.Valid)
            {
                throw new ValidationException("Token already used.");
            }

            token.UserId = command.UserId;
            await _unitOfWork.UpdateAsync(token);

            await _eventPublisher.PublishAsync(new PartnerTokenRegisteredEvent(token));
        }
    }
}