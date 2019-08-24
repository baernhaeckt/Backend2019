using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [Route("api/users/{userId}/points")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<PointResponse> Get(Guid userId)
        {
            return new[]
            {
                new PointResponse()
                {
                    Text = "Bought some weed",
                    Value = 15,
                    MetaData = new List<MetaDataResponse>()
                    {
                        new MetaDataResponse { Key = "Type", Value = "Shopping" },
                        new MetaDataResponse { Key = "Company", Value = "That shady dealer" },
                    }
                }
            };
        }
    }
}