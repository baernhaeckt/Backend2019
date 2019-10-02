using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities.Awards;
using Backend.Core.Extensions;
using Backend.Core.Features.Awards.Queries;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Awards.Controllers
{
    [Route("api/awards")]
    [ApiController]
    public class AwardsController : ControllerBase
    {
        private readonly IQueryPublisher _queryPublisher;

        public AwardsController(IQueryPublisher queryPublisher) => _queryPublisher = queryPublisher;

        [HttpGet]
        public async Task<IEnumerable<Award>> GetAsync()
        {
            UserAwardsQueryResult result = await _queryPublisher.ExecuteAsync(new UserAwardsQuery(HttpContext.User.Id()));
            return result.Awards;
        }
    }
}