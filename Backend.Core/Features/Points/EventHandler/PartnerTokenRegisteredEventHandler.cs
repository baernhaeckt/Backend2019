using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Publishing;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Points.EventHandler
{
    internal class PartnerTokenRegisteredEventHandler : ISubscriber
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IUnitOfWork _unitOfWork;

        public PartnerTokenRegisteredEventHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task ExecuteAsync(PartnerTokenRegisteredEvent @event)
        {
            (Guid id, int points, double co2Saving) = await _unitOfWork
                .GetByIdOrDefaultAsync<User, Tuple<Guid, int, double>>(
                    @event.UserId, u => new Tuple<Guid, int, double>(u.Id, u.Points, u.Co2Saving));

            Token token = await _unitOfWork.GetByIdOrThrowAsync<Token>(@event.TokenId);

            await _unitOfWork.UpdateAsync<User>(id, new
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
            });

            // TODO: Refactor for performance reasons.
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(@event.UserId);
            await _eventPublisher.PublishAsync(new UserNewPointsEvent(user, token.Points, token.Co2Saving));
        }
    }
}