using Backend.Tests.Integration.Utilities;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Integration
{
    public class Security : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly ITestOutputHelper _output;

        public Security(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _output = testOutputHelper;
        }

        [Fact]
        public async Task GetProfileControllerProfile_NormalUser_Ok()
        {
            var client = _factory.CreateClient();

            _output.WriteLine("Sign in with the user");
            await client.SignIn("user@leaf.ch", "user");

            HttpResponseMessage response = await client.GetAsync("api/profile");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetEventControlllerPointsReceived_NormalUser_NOk()
        {
            var client = _factory.CreateClient();
            
            await client.SignIn("user@leaf.ch", "user");

            HttpResponseMessage response = await client.GetAsync("api/events/PointsReceived");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetEventControlllerPointsReceived_Admin_Ok()
        {
            var client = _factory.CreateClient();

            await client.SignIn("admin@leaf.ch", "1234");

            HttpResponseMessage response = await client.GetAsync("api/events/PointsReceived");
            response.EnsureSuccessStatusCode();
        }
    }
}
