using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Extensions;
using Backend.Core.Features.Awards.Models;
using Backend.Infrastructure.Persistence.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Awards.Controllers
{
    [Route("api/awards")]
    [ApiController]
    public class AwardsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AwardsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<AwardsResponse>> GetAsync()
        {
            User currentUser = await _unitOfWork.GetByIdOrDefaultAsync<User>(HttpContext.User.Id());

            return currentUser.Awards.Select(a => new AwardsResponse
            {
                Title = a.Title,
                Kind = a.Kind.ToString()
            });
        }
    }
}