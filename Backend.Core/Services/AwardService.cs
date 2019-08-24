using AspNetCore.MongoDB;
using Backend.Database;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Services.Awards
{
    public class AwardService
    {
        private readonly IMongoOperation<Token> _tokenRepository;
        private readonly IMongoOperation<User> _userRepository;

        public AwardService(IMongoOperation<Token> tokenRepository, IMongoOperation<User> userRepository)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
        }

        public async Task CheckForNewAwardsAsync(Token newToken)
        {
            var allTokensFromUser = _tokenRepository.GetQuerableAsync().Where(t => t.UserId == newToken.UserId);
            var user = await _userRepository.GetByIdAsync(newToken.UserId);

            if(allTokensFromUser.Count() > 1 && user.Awards.All(a => a.Kind != AwardKind.FirstLogin))
            {
                // This is the first token the user gets, so this is worth an award.
                user.Awards.Any(a => a.Kind == AwardKind.FirstLogin);
                user.Awards.Add(new FirstTokenAward());
                await _userRepository.UpdateAsync(user.Id, user);
            }
        }
    }
}
