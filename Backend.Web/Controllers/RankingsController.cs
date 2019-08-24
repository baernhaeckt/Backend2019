using Backend.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Web.Controllers
{
    [Route("api/rankings")]
    [ApiController]
    public class RankingsController : ControllerBase
    {
        [HttpGet("global")]
        public IEnumerable<UserResponse> GetGlobal()
        {
            return Enumerable.Empty<UserResponse>();
        }

        [HttpGet("local")]
        public IEnumerable<UserResponse> GetLocal()
        {
            return Enumerable.Empty<UserResponse>();
        }

        [HttpGet("friends")]
        public IEnumerable<UserResponse> GetFriends()
        {
            return Enumerable.Empty<UserResponse>();
        }
    }
}