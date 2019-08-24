using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Services;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("seed")]
        public void Seed()
        {
            _sufficientTypeService.GenerateSufficientTypes();
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
    }
}