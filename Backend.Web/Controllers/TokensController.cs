using Backend.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly TokenService _tokenGenerationService;

        public TokensController(TokenService tokenGenerationService)
        {
            _tokenGenerationService = tokenGenerationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Get(Guid partnerId) => await _tokenGenerationService.GenerateForPartnerAsync(partnerId);

        [HttpPost]
        public async Task PostAsync(Guid tokenGuid) => await _tokenGenerationService.AssignTokenToUserAsync(tokenGuid);
    }
}