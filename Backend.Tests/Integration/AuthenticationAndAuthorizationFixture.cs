using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Tests.Integration.Utilities.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class AuthenticationAndAuthorizationFixture : IClassFixture<TestContext>
    {
        private readonly TestContext _context;

        private readonly ITestOutputHelper _output;

        public AuthenticationAndAuthorizationFixture(TestContext context, ITestOutputHelper testOutputHelper)
        {
            _context = context;
            _output = testOutputHelper;
        }

        [Fact]
        public async Task GetProfileControllerProfile_NormalUser_Ok()
        {
            HttpClient client = _context.CreateClient();

            _output.WriteLine("Sign in with the user");
            await client.SignIn(TestCredentials.User1, TestCredentials.User1Password);

            HttpResponseMessage response = await client.GetAsync(new Uri("api/profile", UriKind.Relative));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetEventControllerPointsReceived_NormalUser_NOk()
        {
            HttpClient client = _context.CreateClient();

            await client.SignIn(TestCredentials.User1, TestCredentials.User1Password);

            HttpResponseMessage response = await client.GetAsync(new Uri("api/events/PointsReceived", UriKind.Relative));
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task GetEventControllerPointsReceived_Admin_Ok()
        {
            HttpClient client = _context.CreateClient();

            await client.SignIn("admin@leaf.ch", "1234");

            HttpResponseMessage response = await client.GetAsync(new Uri("api/events/PointsReceived", UriKind.Relative));
            response.EnsureSuccessStatusCode();
        }
    }
}