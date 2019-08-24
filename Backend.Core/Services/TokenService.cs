using AspNetCore.MongoDB;
using Backend.Core;
using Backend.Core.Security.Extensions;
using Backend.Database;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class TokenService
    {
        private readonly IMongoOperation<Token> _tokenRepository;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public TokenService(IMongoOperation<Token> tokenRepository, ClaimsPrincipal claimsPrincipal)
        {
            _tokenRepository = tokenRepository;
            _claimsPrincipal = claimsPrincipal;
        }

        public async Task<string> GenerateForPartnerAsync(Guid partnerId)
        {

            if (partnerId != Guid.Parse("ccc14b11-5922-4e3e-bb54-03e71facaeb3"))
            {
                throw new WebException("Partner doesn ont exist.", System.Net.HttpStatusCode.BadRequest);
            }

            var token = new Token();
            token.Points = 10;
            token.CreatedDate = DateTime.Now;
            token.Text = "Blabla";
            token.Value = Guid.NewGuid();

            await _tokenRepository.InsertOneAsync(token);

            return token.Value.ToString();
        }

        public void AssignTokenToUser(Guid tokenGuid)
        {
            var token = _tokenRepository.GetQuerableAsync().SingleOrDefault(t => t.Value == tokenGuid);
            if (token == null)
            {
                throw new WebException("No token found.", System.Net.HttpStatusCode.NotFound);
            }

            if (token.Valid)
            {
                token.UserId = _claimsPrincipal.Id();
            }

            _tokenRepository.UpdateAsync(token.Id, token);
        }
    }
}
