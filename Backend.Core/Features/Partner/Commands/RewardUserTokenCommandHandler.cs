using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Publishing;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Partner.Commands
{
    internal class RewardUserTokenCommandHandler : ISubscriber
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IUnitOfWork _unitOfWork;

        public RewardUserTokenCommandHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task ExecuteAsync(RewardUserTokenCommand command)
        {
            Token token = await _unitOfWork.GetByIdOrThrowAsync<Token>(command.TokenId);
            if (token.IsSingleUse && token.UsedBy.Any())
            {
                throw new ValidationException("Token has already been used.");
            }

            // TOD=: Actually, this here is a race condition! Solve it with version..
            await _unitOfWork.UpdateAsync<Token>(command.TokenId, new { UsedBy = new List<Guid> { command.UserId } });

            await _eventPublisher.PublishAsync(new PartnerTokenRegisteredEvent(command.UserId, command.TokenId));
        }
    }
}