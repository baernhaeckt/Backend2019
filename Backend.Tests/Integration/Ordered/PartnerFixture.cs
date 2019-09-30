using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Data.Testing;
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

        public PartnerFixture(OrderedTestContext context)
        {
            _context = context;
        }

        [Order(0)]
        [Fact]
        public async Task UsersLogin_PartnerLoginSuccessful()
        {
            await _context.PartnerHttpClient.SignIn(TestCredentials.Partner, TestCredentials.PartnerPassword);
        }

        [Order(1)]
        [Fact]
        public async Task TokensCreate_ValidPartnerId_Ok()
        {
            IList<string> partnerIds = new List<string>
            {
                "ccc14b11-5922-4e3e-bb54-03e71facaeb3",
                "acc14b11-5922-4e3e-bb54-03e71facaeb3",
                "bcc14b11-5922-4e3e-bb54-03e71facaeb3",
                "ccc14b11-5922-4e3e-bb54-03e71facaeb3"
            };

            foreach (string partnerId in partnerIds)
            {
                var url = new Uri("api/tokens?partnerId=" + partnerId, UriKind.Relative);
                HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string tokenValue = await response.Content.ReadAsStringAsync();
                Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
                _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());
            }
        }

        [Order(1)]
        [Fact]
        public async Task GetToken_InvalidPartnerId_NOk()
        {
            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(new Uri("api/tokens?partnerId=" + "abd14b11-5922-4e3e-bb54-03e72facaeb3", UriKind.Relative));
            response.EnsureNotSuccessStatusCode();
        }
    }
}