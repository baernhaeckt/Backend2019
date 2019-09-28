using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Integration
{

    public class BasicTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly ITestOutputHelper _testOutputHelper;

        public BasicTests(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task SmokeTests()
        {
            var client = _factory.CreateClient();

            string url = "/api/users/Register?email=marc.sallin@outlook.com";
            var response = await client.PostAsync(url, null);
            
            response.EnsureSuccessStatusCode();
            url = "api/users/Login?email=marc.sallin@outlook.com&password=1234";
            var responseWithJwt = await client.PostAsync(url, null);
            responseWithJwt.EnsureSuccessStatusCode();
            _testOutputHelper.WriteLine(await responseWithJwt.Content.ReadAsStringAsync());

        }
    }
}