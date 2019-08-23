using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<PrivateUserResponse> Get()
        {
            return new[]
            {
                new PrivateUserResponse()
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
                ,new PrivateUserResponse()
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
                ,new PrivateUserResponse()
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