using Backend.Core.Newsfeed;
using Backend.Database;
using Backend.Database.Abstraction;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class AwardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventStream _eventStream;

        public AwardService(IUnitOfWork unitOfWork, IEventStream eventStream)
        {
            _unitOfWork = unitOfWork;
            _eventStream = eventStream;
        }

        public async Task CheckForNewAwardsAsync(User user)
        {
            // Yes. We are aware that this method contains horrible code.
            var allTokensFromUser = (await _unitOfWork.GetTokensFromUser(user.Id)).ToList();

            Award newAward = null;
            if (allTokensFromUser.Count() > 1 && user.Awards.All(a => a.Kind != AwardKind.Onboarding))
            {
                // This is the first token the user gets, so this is worth an award.
                newAward = new OnBoardingAward();
            }

            if (allTokensFromUser.Count(t => t.SufficientType.Title == "Verpackung") >= 2 && user.Awards.All(a => a.Kind != AwardKind.TrashHero))
            {
                newAward = new TrashHeroAward();
            }

            if (newAward != null)
            {
                user.Awards.Add(newAward);
                await _unitOfWork.UpdateAsync(user);

                await _eventStream.PublishAsync(new BadgeReceivedEvent(user, newAward));
                await _eventStream.PublishAsync(new FriendBadgeReceivedEvent(user, newAward));
            }
        }
    }
}
