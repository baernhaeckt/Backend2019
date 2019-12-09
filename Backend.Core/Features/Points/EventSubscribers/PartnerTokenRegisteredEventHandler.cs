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

            // Use PointHistory to get the current points/Co2Savings and take this as a chance to sync, if there is a mismatch between those values and the PointHistory.
            (Guid id, int points, double co2Saving, string displayName) = await _unitOfWork.GetByIdOrThrowAsync<User, Tuple<Guid, int, double, string>>(
                    @event.UserId, u => new Tuple<Guid, int, double, string>(u.Id, u.PointHistory.Sum(pa => pa.Point), u.PointHistory.Sum(pa => pa.Co2Saving), u.DisplayName));

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

            await _eventPublisher.PublishAsync(new UserNewPointsEvent(id, token.Points, token.Co2Saving, displayName));

            _logger.HandlePartnerTokenRegisteredEventSuccessful(@event.UserId, @event.TokenId);
        }
    }
}