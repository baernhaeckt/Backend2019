using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.PointsAndAwards.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.PointsAndAwards.Controllers
{
    [Route("api/users/{userId}/points")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly PointService _pointService;

        public PointsController(PointService pointService)
        {
            _pointService = pointService;
        }

        [HttpGet]
        public async Task<IEnumerable<PointResponse>> GetAsync(Guid userId)
        {
            IEnumerable<PointAction> pointList = await _pointService.PointHistory(userId);
            return pointList.OrderByDescending(p => p.Date).Take(25).Select(p => new PointResponse
            {
                Date = p.Date,
                Id = p.Id,
                Text = p.Action,
                Value = p.Point,
                Co2Saving = p.Co2Saving,
                SufficientType = p.SufficientType
            }).ToList();
        }
    }
}