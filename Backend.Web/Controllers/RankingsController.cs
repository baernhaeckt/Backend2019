using AspNetCore.MongoDB;
using Backend.Core.Services;
using Backend.Database;
using Backend.Models;
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

        public FriendsService FriendsService { get; }

        public RankingsController(
            FriendsService friendsService,
            IMongoOperation<User> userRepository, 
            IMongoOperation<Token> tokenRepository)
        {
            FriendsService = friendsService;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        [HttpGet("global")]
        public async Task<IEnumerable<UserResponse>> GetGlobalAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var results = CreateResult(users);

            return results.OrderByDescending(r => r.Points);
        }

        [HttpGet("local")]
        public IEnumerable<UserResponse> GetLocal(string zip)
        {
            var users = _userRepository.GetQuerableAsync()
                .Where(u => u.Location.Zip == zip);

            var results = CreateResult(users);

            return results.OrderByDescending(r => r.Points);
        }

        [HttpGet("friends")]
        public IEnumerable<UserResponse> GetFriends()
        {
            var results = CreateResult(FriendsService.Friends.ToList());

            return results.OrderByDescending(u => u.Points);
        }

        private IEnumerable<UserResponse> CreateResult(IEnumerable<User> users)
        {
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

            return results;
        }
    }
}