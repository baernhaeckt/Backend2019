using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Tests.Integration.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class AuthenticationAndAuthorizationFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        private readonly ITestOutputHelper _output;

        public AuthenticationAndAuthorizationFixture(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _output = testOutputHelper;
        }

        [Fact]
        public async Task GetProfileControllerProfile_NormalUser_Ok()
        {
            HttpClient client = _factory.CreateClient();

            _output.WriteLine("Sign in with the user");
            await client.SignIn("user@leaf.ch", "user");

            HttpResponseMessage response = await client.GetAsync(new Uri("api/profile", UriKind.Relative));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetEventControllerPointsReceived_NormalUser_NOk()
        {
            HttpClient client = _factory.CreateClient();

            await client.SignIn("user@leaf.ch", "user");

            HttpResponseMessage response = await client.GetAsync(new Uri("api/events/PointsReceived", UriKind.Relative));
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetEventControllerPointsReceived_Admin_Ok()
        {
            HttpClient client = _factory.CreateClient();

            await client.SignIn("admin@leaf.ch", "1234");

            HttpResponseMessage response = await client.GetAsync(new Uri("api/events/PointsReceived", UriKind.Relative));
            response.EnsureSuccessStatusCode();
        }
    }
}