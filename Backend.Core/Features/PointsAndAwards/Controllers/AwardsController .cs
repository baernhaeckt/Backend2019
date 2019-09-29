﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.PointsAndAwards.Models;
using Backend.Database;
using Backend.Database.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.PointsAndAwards.Controllers
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
            var currentUser = await _unitOfWork.GetAsync<User>(HttpContext.User.Id());
            
            return currentUser.Awards.Select(a => new AwardsResponse
            {
                Title = a.Title,
                Kind = a.Kind.ToString()
            });
        }
    }
}