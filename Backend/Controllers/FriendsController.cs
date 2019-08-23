using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<UserResponse> Get()
        {
            return new[]
            {
                new UserResponse()
                {
                    Id = Guid.NewGuid(),
                    Email = "friend1@test.ch",
                    Points = 150,
                    Location = new LocationResponse()
                    {
                        Latitude = 46.941060,
                        Longitude = 7.442725
                    }
                }
                ,new UserResponse()
                {
                    Id = Guid.NewGuid(),
                    Email = "friend2@test.ch",
                    Points = 150,
                    Location = new LocationResponse()
                    {
                        Latitude = 46.941060,
                        Longitude = 7.442725
                    }
                }
                ,new UserResponse()
                {
                    Id = Guid.NewGuid(),
                    Email = "friend3@test.ch",
                    Points = 150,
                    Location = new LocationResponse()
                    {
                        Latitude = 46.941060,
                        Longitude = 7.442725
                    }
                }
            };
        }
    }
}