using System;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Tests.Integration.Utilities;
using Xunit;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class PartnerFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public PartnerFixture(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetToken_ValidPartnerId_Ok()
        {
            HttpClient client = _factory.CreateClient();

            await client.SignIn(TestCredentials.Partner, TestCredentials.PartnerPassword);

            HttpResponseMessage response = await client.GetAsync(new Uri("api/tokens?partnerId=" + "ccc14b11-5922-4e3e-bb54-03e71facaeb3", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            string token = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public async Task GetToken_InvalidPartnerId_NOk()
        {
            HttpClient client = _factory.CreateClient();

            await client.SignIn(TestCredentials.Partner, TestCredentials.PartnerPassword);

            HttpResponseMessage response = await client.GetAsync(new Uri("api/tokens?partnerId=" + "abd14b11-5922-4e3e-bb54-03e72facaeb3", UriKind.Relative));
            response.EnsureNotSuccessStatusCode();
        }
    }
}