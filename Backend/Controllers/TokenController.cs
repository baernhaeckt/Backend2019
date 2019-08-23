using Microsoft.AspNetCore.Mvc;
using System;

namespace Backend.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        [HttpGet]
        public TokenResponse Get(Guid tokenGuid)
        {
            return new TokenResponse()
            {
                Id = Guid.NewGuid(),
                Text = "default text",
                Points = 100,
                Valid = true,
                User = new PrivateUserResponse
                {
                    Id = Guid.NewGuid(),
                    Email = "test@mail.com",
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