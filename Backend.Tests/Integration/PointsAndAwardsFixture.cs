using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.PointsAndAwards.Models;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Tests.Integration.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class PointsAndAwardsFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        private readonly ITestOutputHelper _output;

        public PointsAndAwardsFixture(CustomWebApplicationFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task GetProfileControllerProfile_NormalUser_Ok()
        {
            HttpClient client = _factory.CreateClient();

            _output.WriteLine("Sign in with the user: " + TestCredentials.Partner);
            await client.SignIn(TestCredentials.Partner, "partner");

            _output.WriteLine("Generate partner tokens");
            IList<string> partnerIds = new List<string>
            {
                "ccc14b11-5922-4e3e-bb54-03e71facaeb3",
                "acc14b11-5922-4e3e-bb54-03e71facaeb3",
                "bcc14b11-5922-4e3e-bb54-03e71facaeb3",
                "ccc14b11-5922-4e3e-bb54-03e71facaeb3"
            };

            IList<string> tokenValues = new List<string>();
            foreach (string partnerId in partnerIds)
            {
                _output.WriteLine("Generate token for partner: " + partnerId);
                var url = new Uri("api/tokens?partnerId=" + partnerId, UriKind.Relative);
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                tokenValues.Add(await response.Content.ReadAsStringAsync());
            }

            string email = await client.CreateUserAndSignIn();
            _output.WriteLine("Sign in with the user: " + email);

            foreach (string tokenValue in tokenValues)
            {
                _output.WriteLine("Use token: " + tokenValue);
                var url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
                HttpResponseMessage response = await client.PostAsync(url, null);
                response.EnsureSuccessStatusCode();
            }

            {
                _output.WriteLine("Has the 'On-boarding' and 'TrashHero'-award");
                var url = new Uri("api/awards", UriKind.Relative);
                HttpResponseMessage response = await client.GetAsync(url);
                List<AwardsResponse> awards = await response.OnSuccessDeserialize<List<AwardsResponse>>();
                Assert.Equal(2, awards.Count);
            }
        }
    }
}