using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Extensions;
using Backend.Core.Features.Points.Models;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Points.Controllers
{
    [Route("api/points")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly IReader _reader;

        public PointsController(IReader reader) => _reader = reader;

        [HttpGet]
        public async Task<IEnumerable<PointResponse>> Get()
        {
            // TODO: Refactor to query.
            IEnumerable<PointAction> pointList = await _reader.GetByIdOrThrowAsync<User, IEnumerable<PointAction>>(HttpContext.User.Id(), u => u.PointHistory.Take(25));
            return pointList.OrderByDescending(p => p.Date).Select(p => new PointResponse
            {
                Date = p.Date,
                Text = p.Action,
                Value = p.Point,
                Co2Saving = p.Co2Saving,
                SufficientType = p.SufficientType
            }).ToList();
        }
    }
}