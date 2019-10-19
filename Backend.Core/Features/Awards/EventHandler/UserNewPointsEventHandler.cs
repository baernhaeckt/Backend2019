using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Awards;
using Backend.Core.Events;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Publishing;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Awards.EventHandler
{
    internal class UserNewPointsEventHandler : ISubscriber
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IUnitOfWork _unitOfWork;

        public UserNewPointsEventHandler(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task ExecuteAsync(UserNewPointsEvent @event)
        {
            IList<Award> newAwards = new List<Award>();
            if (@event.User.PointHistory.Count >= 1
                && @event.User.Awards.All(a => !(a is OnBoardingAward)))
            {
                // This is the first token the user gets, so this is worth an award.
                var award = new OnBoardingAward();
                newAwards.Add(award);
            }

            if (@event.User.PointHistory.Count(t => string.Equals(t.SufficientType.Title, "Verpackung", StringComparison.OrdinalIgnoreCase)) >= 2
                && @event.User.Awards.All(a => !(a is TrashHeroAward)))
            {
                var award = new TrashHeroAward();
                newAwards.Add(award);
            }

            if (newAwards.Any())
            {
                await _unitOfWork.UpdateAsync<User>(@event.User.Id, new { Awards = newAwards });

                foreach (Award newAward in newAwards)
                {
                    await _eventPublisher.PublishAsync(new UserNewAwardEvent(@event.User, newAward));
                }
            }
        }
    }
}