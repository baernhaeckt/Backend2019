using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.Baseline.Queries;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Baseline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SufficientTypeController : ControllerBase
    {
        private readonly IQueryPublisher _queryPublisher;

        public SufficientTypeController(IQueryPublisher queryPublisher) => _queryPublisher = queryPublisher;

        [HttpGet("baseline")]
        public async Task<IEnumerable<SufficientTypesQueryResult>> GetBaseLinePoints() => await _queryPublisher.ExecuteAsync(new AveragePointsPerSufficientTypesQuery());

        [HttpGet("user")]
        public async Task<IEnumerable<SufficientTypesQueryResult>> GetUserPoints() => await _queryPublisher.ExecuteAsync(new PointsPerSufficientTypesQuery(User.Id()));
    }
}