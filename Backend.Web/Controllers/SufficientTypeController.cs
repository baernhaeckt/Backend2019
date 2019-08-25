using Backend.Core.Services;
using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SufficientTypeController : ControllerBase
    {
        private readonly SufficientTypeService _sufficientTypeService;

        public SufficientTypeController(SufficientTypeService sufficientTypeService)
        {
            _sufficientTypeService = sufficientTypeService;
        }

        [HttpGet("baseline")]
        public async Task<IEnumerable<BaselineResponse>> GetBaseLinePoints()
        {
            var sufficientTypes = await _sufficientTypeService.GetSufficientTypes();
            return sufficientTypes.Select(s => new BaselineResponse
            {
                Title = s.Title,
                BaseLinePoint = s.BaselinePoint,
                BaselineCo2Saving = s.BaselineCo2Saving
            });
        }

        [HttpGet("user")]
        public async Task<IEnumerable<UserSufficientResponse>> GetUserPoints()
        {
            IEnumerable<UserSufficientType> userSufficientTypes = await _sufficientTypeService.GetSufficientTypesFromUser();
            return userSufficientTypes.Select(u => new UserSufficientResponse
            {
                Title = u.Title,
                Point = u.Point,
                Co2Saving = u.Co2Saving
            });
        }
    }
}