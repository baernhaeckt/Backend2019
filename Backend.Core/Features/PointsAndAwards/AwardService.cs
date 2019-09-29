using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Awards;
using Backend.Core.Extensions;
using Backend.Core.Features.Newsfeed;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Features.PointsAndAwards
{
    public class AwardService
    {
        private readonly IEventFeed _eventFeed;

        private readonly IUnitOfWork _unitOfWork;

        public AwardService(IUnitOfWork unitOfWork, IEventFeed eventFeed)
        {
            _unitOfWork = unitOfWork;
            _eventFeed = eventFeed;
        }

        public async Task CheckForNewAwardsAsync(User user)
        {
            // Yes. We are aware that this method contains horrible code.
            List<Token> allTokensFromUser = (await _unitOfWork.GetTokensFromUser(user.Id)).ToList();

            Award? newAward = null;
            if (allTokensFromUser.Count > 1 && user.Awards.All(a => a.Kind != AwardKind.Onboarding))
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

                await _eventFeed.PublishAsync(new BadgeReceivedNewsfeedEvent(user, newAward));
                await _eventFeed.PublishAsync(new FriendNewsfeedBadgeReceivedEvent(user, newAward));
            }
        }
    }
}