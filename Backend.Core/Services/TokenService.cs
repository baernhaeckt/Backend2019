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

            Token token = null;
            if (partnerId == Guid.Parse("ccc14b11-5922-4e3e-bb54-03e71facaeb3"))
            {
                token = new Token();
                token.Points = 10;
                token.CreatedDate = DateTime.Now;
                token.Text = "Blabla";
                token.Value = Guid.NewGuid();
            }
            else if (partnerId == Guid.Parse("bcc14b11-5922-4e3e-bb54-03e71facaeb3"))
            {
                token = new Token();
                token.Points = 5;
                token.CreatedDate = DateTime.Now;
                token.Text = "Blabla2";
                token.Value = Guid.NewGuid();
            }
            else if (partnerId == Guid.Parse("acc14b11-5922-4e3e-bb54-03e71facaeb3"))
            {
                token = new Token();
                token.Points = 15;
                token.CreatedDate = DateTime.Now;
                token.Text = "Blabla3";
                token.Value = Guid.NewGuid();
            }

            if (token != null)
            {
                await _tokenRepository.InsertOneAsync(token);
                return token.Value.ToString();
            }

            throw new WebException("Partner doesn ont exist.", System.Net.HttpStatusCode.BadRequest);
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
