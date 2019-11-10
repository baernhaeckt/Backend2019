using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.Points.Queries;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Points.Controllers
{
    [Route("api/rankings")]
    [ApiController]
    public class RankingsController : ControllerBase
    {
        private readonly IQueryPublisher _queryPublisher;

        public RankingsController(IQueryPublisher queryPublisher) => _queryPublisher = queryPublisher;

        [HttpGet("global")]
        public async Task<IEnumerable<RankingQueryResult>> GetGlobal()
        {
            var rankingQuery = new RankingQuery();
            return await _queryPublisher.ExecuteAsync(rankingQuery);
        }

        [HttpGet("local")]
        public async Task<IEnumerable<RankingQueryResult>> GetLocal(string zip)
        {
            var rankingQuery = new RankingByZipQuery(zip);
            return await _queryPublisher.ExecuteAsync(rankingQuery);
        }

        [HttpGet("friends")]
        public async Task<IEnumerable<RankingQueryResult>> GetFriends()
        {
            var rankingQuery = new RankingForUserFriendsQuery(HttpContext.User.Id());
            return await _queryPublisher.ExecuteAsync(rankingQuery);
        }

        [HttpGet("summary")]
        public async Task<RankingSummaryQueryResult> GetSummary() => await _queryPublisher.ExecuteAsync(new RankingSummaryQuery(HttpContext.User.Id()));
    }
}