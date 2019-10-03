using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Partner.Data.Testing;
using Backend.Tests.Integration.Utilities.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Backend.Tests.Integration
{
    [Order(2)]
    [Collection("IntegrationTests")]
    [Trait("Category", "Integration")]
    public class PartnerFixture
    {
        private readonly OrderedTestContext _context;

        public PartnerFixture(OrderedTestContext context) => _context = context;

        [Order(0)]
        [Fact]
        public async Task TokenIssuersLogin_Successful()
        {
            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id2, TokenIssuerTestCredentials.Secret2);
        }

        [Order(1)]
        [Fact]
        public async Task TokensCreate_ValidPartnerId_Ok()
        {
            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id1, TokenIssuerTestCredentials.Secret1);
            var url = new Uri("api/tokens?tokenType=VerpAckUng", UriKind.Relative);

            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string tokenValue = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
            _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());

            response = await _context.PartnerHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            tokenValue = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
            _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());

            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id3, TokenIssuerTestCredentials.Secret3);
            url = new Uri("api/tokens?tokenType=TeIleN", UriKind.Relative);

            response = await _context.PartnerHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            tokenValue = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
            _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());

            url = new Uri("api/tokens?tokenType=multiuse", UriKind.Relative);
            response = await _context.PartnerHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            tokenValue = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
            _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());
        }

        [Order(1)]
        [Fact]
        public async Task GetToken_InvalidTokenType_NOk()
        {
            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(new Uri("api/tokens?tokenType=bla", UriKind.Relative));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Order(2)]
        public async Task TokensUse_Successful()
        {
            foreach (string tokenValue in _context.PartnerGeneratedTokens)
            {
                var url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
                HttpResponseMessage response = await _context.NewTestUserHttpClient.PostAsync(url, null);
                response.EnsureSuccessStatusCode();
            }
        }

        [Order(3)]
        [Fact]
        public async Task UseToken_SingleUseAlreadyUsed_NOk()
        {
            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id3, TokenIssuerTestCredentials.Secret3);
            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(new Uri("api/tokens?tokenType=TeIleN", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            string tokenValue = await response.Content.ReadAsStringAsync();

            var url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
            response = await _context.NewTestUserHttpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();

            response = await _context.NewTestUserHttpClient.PostAsync(url, null);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Order(4)]
        [Fact]
        public async Task UseToken_MultiUseAlreadyUsed_Ok()
        {
            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id3, TokenIssuerTestCredentials.Secret3);
            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(new Uri("api/tokens?tokenType=multiuse", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            string tokenValue = await response.Content.ReadAsStringAsync();

            var url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
            response = await _context.NewTestUserHttpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();

            response = await _context.NewTestUserHttpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();

            response = await _context.NewTestUserHttpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
        }
    }
}