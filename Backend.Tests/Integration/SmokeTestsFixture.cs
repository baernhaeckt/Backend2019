using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Tests.Integration.Utilities;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class SmokeTestsFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        private readonly ITestOutputHelper _output;

        public SmokeTestsFixture(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _output = testOutputHelper;
        }

        [Fact]
        public async Task Execute()
        {
            HttpClient client = _factory.CreateClient();

            string email = DataGenerator.RandomEmail();
            _output.WriteLine("Register new user: " + email);

            var url = new Uri("api/users/Register?email=" + email, UriKind.Relative);
            HttpResponseMessage response = await client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();

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
                url = new Uri("api/tokens?partnerId=" + partnerId, UriKind.Relative);
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                tokenValues.Add(await response.Content.ReadAsStringAsync());
            }

            _output.WriteLine("Sign in with the user: " + email);
            await client.SignIn(email, "1234");

            foreach (string tokenValue in tokenValues)
            {
                _output.WriteLine("Use token: " + tokenValue);
                url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
                response = await client.PostAsync(url, null);
                response.EnsureSuccessStatusCode();
            }

            _output.WriteLine("Get global ranking");
            url = new Uri("api/rankings/global", UriKind.Relative);
            response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            _output.WriteLine("Has the 'On-boarding' and 'TrashHero'-award");
            url = new Uri("api/awards", UriKind.Relative);
            response = await client.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            dynamic awardsResponse = JsonConvert.DeserializeObject(content);
            Assert.Equal(2, awardsResponse.Count);

            _output.WriteLine("SufficientTypes Baseline");
            url = new Uri("api/sufficienttype/baseline", UriKind.Relative);
            response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}