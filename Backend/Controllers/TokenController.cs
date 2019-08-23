using Microsoft.AspNetCore.Mvc;
using System;

namespace Backend.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
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
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "test@mail.com"
                }
            };
        }
    }
}