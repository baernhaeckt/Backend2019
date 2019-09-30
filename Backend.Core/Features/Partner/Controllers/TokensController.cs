using System;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Partner.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    [Authorize(Roles = Roles.Partner)]
    public class TokensController : ControllerBase
    {
        private readonly TokenService _tokenGenerationService;

        public TokensController(TokenService tokenGenerationService)
        {
            _tokenGenerationService = tokenGenerationService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get(Guid partnerId)
        {
            return await _tokenGenerationService.GenerateForPartnerAsync(partnerId);
        }
    }
}