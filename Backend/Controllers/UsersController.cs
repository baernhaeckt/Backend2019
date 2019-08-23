using Microsoft.AspNetCore.Mvc;
using System;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("current")]
        public PrivateUserResponse Current()
        {
            return new PrivateUserResponse()
            {
                Id = Guid.NewGuid(),
                Email = "current@test.ch",
                Points = 150,
                Location = new LocationResponse()
                {
                    Latitude = 46.941060,
                    Longitude = 7.442725
                }
            };
        }

        [HttpGet]
        public PrivateUserResponse Get(Guid guid)
        {
            return new PrivateUserResponse()
            {
                Id = guid,
                Email = "user@test.ch",
                Points = 150,
                Location = new LocationResponse()
                {
                    Latitude = 46.941060,
                    Longitude = 7.442725
                }
            };
        }

        [HttpPost]
        public LoginResponse Login(string email, string password)
        {
            return new LoginResponse { Token = string.Empty };
        }
    }
}