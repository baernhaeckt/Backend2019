using Backend.Core.Services;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Web.Controllers
{
    [Route("api/awards")]
    [ApiController]
    public class AwardsController : ControllerBase
    {
        private readonly UserService _userService;

        public AwardsController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public IEnumerable<AwardsResponse> Get()
        {
            return _userService.CurrentUser.Awards.Select(a => new AwardsResponse
            {
                Title = a.Title,
                Kind = a.Kind.ToString()
            });
        }
    }
}