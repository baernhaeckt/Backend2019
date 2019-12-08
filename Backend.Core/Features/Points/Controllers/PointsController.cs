using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.Points.Queries;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Points.Controllers
{
    [Route("api/points")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly IQueryPublisher _queryPublisher;

        public PointsController(IQueryPublisher queryPublisher) => _queryPublisher = queryPublisher;

        [HttpGet]
        public async Task<IEnumerable<PointHistoryForUserQueryResult>> Get() => await _queryPublisher.ExecuteAsync(new PointHistoryForUserQuery(User.Id()));
    }
}