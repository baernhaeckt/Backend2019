using AspNetCore.MongoDB;
using Backend.Database;
using Backend.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
    [Route("api/rankings")]
    [ApiController]
    public class RankingsController : ControllerBase
    {
        private readonly IMongoOperation<User> _userRepository;
        private readonly IMongoOperation<Token> _tokenRepository;

        public RankingsController(IMongoOperation<User> userRepository, IMongoOperation<Token> tokenRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        [HttpGet("global")]
        public async Task<IEnumerable<UserResponse>> GetGlobalAsync()
        {
            var users = await _userRepository.GetAllAsync();

            IList<UserResponse> results = new List<UserResponse>(users.Count());

            foreach (var user in users)
            {
                int pointsForUser = _tokenRepository.GetQuerableAsync()
                    .Where(t => t.UserId == user.Id)
                    .Sum(u => u.Points);

                results.Add(new UserResponse
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Points = pointsForUser
                });
            }

            return results.OrderByDescending(r => r.Points);
        }

        [HttpGet("local")]
        public IEnumerable<UserResponse> GetLocal()
        {
            return Enumerable.Empty<UserResponse>();
        }

        [HttpGet("friends")]
        public IEnumerable<UserResponse> GetFriends()
        {
            return Enumerable.Empty<UserResponse>();
        }
    }
}