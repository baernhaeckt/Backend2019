using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Partner;
using Backend.Core.Events;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Publishing;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Points.EventSubscribers
{
    internal class PartnerTokenRegisteredEventHandler : ISubscriber
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly ILogger<PartnerTokenRegisteredEventHandler> _logger;

        private readonly IUnitOfWork _unitOfWork;

        public PartnerTokenRegisteredEventHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher, ILogger<PartnerTokenRegisteredEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task ExecuteAsync(PartnerTokenRegisteredEvent @event)
        {
            _logger.HandlePartnerTokenRegisteredEvent(@event.UserId, @event.TokenId);

            (Guid id, int points, double co2Saving) = await _unitOfWork
                .GetByIdOrThrowAsync<User, Tuple<Guid, int, double>>(
                    @event.UserId, u => new Tuple<Guid, int, double>(u.Id, u.PointHistory.Sum(pa => pa.Point), u.PointHistory.Sum(pa => pa.Co2Saving)));

            Token token = await _unitOfWork.GetByIdOrThrowAsync<Token>(@event.TokenId);

            var updateObject = new
            {
                Points = points + token.Points,
                Co2Saving = co2Saving + token.Co2Saving,
                PointHistory = new List<PointAction>
                {
                    new PointAction
                    {
                        Point = token.Points,
                        Action = token.Text,
                        Co2Saving = token.Co2Saving,
                        SponsorRef = token.Name,
                        SufficientType = new UserSufficientType
                        {
                            Title = token.SufficientType.Title
                        }
                    }
                }
            };

            _logger.PartnerGrantPointsForTokenUpdateUser(updateObject);
            await _unitOfWork.UpdateAsync<User>(id, updateObject);

            // TODO: Refactor for performance reasons.
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(@event.UserId);
            await _eventPublisher.PublishAsync(new UserNewPointsEvent(user, token.Points, token.Co2Saving));

            _logger.HandlePartnerTokenRegisteredEventSuccessful(@event.UserId, @event.TokenId);
        }
    }
}