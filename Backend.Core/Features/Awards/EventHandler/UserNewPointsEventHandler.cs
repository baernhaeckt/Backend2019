using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities.Awards;
using Backend.Core.Events;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Awards.EventHandler
{
    internal class UserNewPointsEventHandler : ISubscriber
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserNewPointsEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(UserNewPointsEvent @event)
        {
            Award? newAward = null;
            if (@event.User.PointActions.Count > 1
                && @event.User.Awards.All(a => a.Kind != AwardKind.Onboarding))
            {
                // This is the first token the user gets, so this is worth an award.
                newAward = new OnBoardingAward();
            }

            if (@event.User.PointActions.Count(t => t.SufficientType.Title == "Verpackung") >= 2
                && @event.User.Awards.All(a => a.Kind != AwardKind.TrashHero))
            {
                newAward = new TrashHeroAward();
            }

            if (newAward != null)
            {
                @event.User.Awards.Add(newAward);
                await _unitOfWork.UpdateAsync(@event.User);
            }
        }
    }
}