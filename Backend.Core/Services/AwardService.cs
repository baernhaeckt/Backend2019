using AspNetCore.MongoDB;
using Backend.Core.Newsfeed;
using Backend.Database;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class AwardService
    {
        private readonly IMongoOperation<Token> _tokenRepository;
        private readonly IMongoOperation<User> _userRepository;
        private readonly IEventStream _eventStream;

        public AwardService(IMongoOperation<Token> tokenRepository, IMongoOperation<User> userRepository, IEventStream eventStream)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _eventStream = eventStream;
        }

        public async Task CheckForNewAwardsAsync(User user)
        {
            // Yes. We are aware that this method contains horrible code.
            var allTokensFromUser = _tokenRepository.GetQuerableAsync().Where(t => t.UserId == user.Id);

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
                await _userRepository.UpdateAsync(user.Id, user);

                await _eventStream.PublishAsync(new BadgeReceivedEvent(user, newAward));
                await _eventStream.PublishAsync(new FriendBadgeReceivedEvent(user, newAward));
            }
        }
    }
}
