using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Backend.Tests.Integration
{
    [Order(6)]
    [Collection("IntegrationTests")]
    [Trait("Category", "Integration")]
    public class BaselineFixture
    {
        private readonly OrderedTestContext _context;

        public BaselineFixture(OrderedTestContext context) => _context = context;

        [Fact]
        public async Task SufficientBaseline_Successful()
        {
            var url = new Uri("api/SufficientType/baseline", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task SufficientTypeUser_Successful()
        {
            var url = new Uri("api/SufficientType/user", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}