using Microsoft.AspNetCore.Mvc;
using System;

namespace Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("current")]
        public UserResponse Current()
        {
            return new UserResponse()
            {
                Id = Guid.NewGuid(),
                Email = "current@test.ch"
            };
        }

        [HttpGet]
        public UserResponse Get(Guid guid)
        {
            return new UserResponse()
            {
                Id = guid,
                Email = "user@test.ch"
            };
        }
    }
}