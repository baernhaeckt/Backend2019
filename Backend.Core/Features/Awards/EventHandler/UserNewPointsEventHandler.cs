using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            if (@event.User.PointActions.Count > 1
                && @event.User.Awards.All(a => a.Kind != AwardKind.Onboarding))
            {
                // This is the first token the user gets, so this is worth an award.
                var award = new OnBoardingAward();
                newAwards.Add(award);
            }

            if (@event.User.PointActions.Count(t => t.SufficientType.Title == "Verpackung") >= 2
                && @event.User.Awards.All(a => a.Kind != AwardKind.TrashHero))
            {
                var award = new TrashHeroAward();
                newAwards.Add(award);
            }

            foreach (Award newAward in newAwards)
            {
                @event.User.Awards.Add(newAward);
                await _eventPublisher.PublishAsync(new UserNewAwardEvent(@event.User, newAward));
            }

            await _unitOfWork.UpdateAsync(@event.User);
        }
    }
}