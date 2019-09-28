using Backend.Tests.Integration.Utilities;
using Bogus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Integration
{
    public class SmokeTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly ITestOutputHelper _output;

        public SmokeTests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _output = testOutputHelper;
        }

        [Fact]
        public async Task Execute()
        {
            var client = _factory.CreateClient();

            string email = RandomEmail();
            _output.WriteLine("New user will be " + email);

            _output.WriteLine("Register a user");
            string url = "/api/users/Register?email=" + email;
            var response = await client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();

            _output.WriteLine("Sign in with the user");
            await client.SignIn(email, "1234");

            IList<string> partnerIds = new List<string>();
            partnerIds.Add("ccc14b11-5922-4e3e-bb54-03e71facaeb3");
            partnerIds.Add("acc14b11-5922-4e3e-bb54-03e71facaeb3");
            partnerIds.Add("bcc14b11-5922-4e3e-bb54-03e71facaeb3");
            partnerIds.Add("ccc14b11-5922-4e3e-bb54-03e71facaeb3");

            IList<string> tokenValues = new List<string>();
            foreach (var partnerId in partnerIds)
            {
                _output.WriteLine("Generate Tokens");
                url = "api/tokens?partnerId=" + partnerId;
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                tokenValues.Add(await response.Content.ReadAsStringAsync());
            }
            
            foreach (var tokenValue in tokenValues)
            {
                _output.WriteLine("Use Tokens");
                url = "api/tokens?tokenGuid=" + tokenValue;
                response = await client.PostAsync(url, null);
                response.EnsureSuccessStatusCode();
            }

            _output.WriteLine("Ranking");
            url = "api/rankings/global";
            response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            _output.WriteLine("Has the 'Onboarding' and 'TrashHero'-award");
            url = "api/awards";
            response = await client.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            dynamic awardsResponse = JsonConvert.DeserializeObject(content);
            Assert.Equal(2, awardsResponse.Count);

            _output.WriteLine("SufficientTypes Baseline");
            url = "api/sufficienttype/baseline";
            response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }

        private string RandomEmail()
        {
            Randomizer.Seed = new Random();
            var faker = new Faker("en");
            return faker.Internet.Email();
        }
    }
}