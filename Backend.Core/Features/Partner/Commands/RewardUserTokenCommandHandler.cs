using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Partner.Commands
{
    internal class RewardUserTokenCommandHandler : CommandHandler<RewardUserTokenCommand>
    {
        private readonly IEventPublisher _eventPublisher;

        public RewardUserTokenCommandHandler(IUnitOfWork unitOfWork, ILogger<RewardUserTokenCommandHandler> logger, IEventPublisher eventPublisher) 
            : base(unitOfWork, logger) => _eventPublisher = eventPublisher;

        public override async Task ExecuteAsync(RewardUserTokenCommand command)
        {
            Logger.InitiateRewardUserToken(command.UserId, command.TokenId);

            Token token = await UnitOfWork.GetByIdOrThrowAsync<Token>(command.TokenId);
            if (token.IsSingleUse && token.UsedBy.Any())
            {
                Logger.RewardUserTokenTokenNotValid(command.UserId, command.TokenId, token.IsSingleUse, token.UsedBy.Any());
                throw new ValidationException("Token has already been used.");
            }

            // TODO: Actually, this here is a race condition! Solve it with version..
            await UnitOfWork.UpdateAsync<Token>(command.TokenId, new { UsedBy = new List<Guid> { command.UserId } });

            await _eventPublisher.PublishAsync(new PartnerTokenRegisteredEvent(command.UserId, command.TokenId));

            Logger.RewardUserTokenSuccessful(command.UserId, command.TokenId);
        }
    }
}