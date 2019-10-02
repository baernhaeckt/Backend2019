using System;
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
            long count = await _unitOfWork.CountAsync<Token>(t => t.Value == command.TokenId && t.UserId == Guid.Empty);
            if (count > 1)
            {
                throw new ValidationException("Token doesn't exist or was already used.");
            }

            await _unitOfWork.UpdateAsync<User>(command.TokenId, new { UserId = command.UserId });

            await _eventPublisher.PublishAsync(new PartnerTokenRegisteredEvent(command.UserId, command.TokenId));
        }
    }
}