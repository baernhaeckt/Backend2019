using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Backend.Tests.Integration
{
    [Order(4)]
    [Collection("IntegrationTests")]
    [Trait("Category", "Integration")]
    public class PointsFixture
    {
        private readonly OrderedTestContext _context;

        public PointsFixture(OrderedTestContext context) => _context = context;

        [Fact]
        public async Task TokensUse_Successful()
        {
            foreach (string tokenValue in _context.PartnerGeneratedTokens)
            {
                var url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
                HttpResponseMessage response = await _context.NewTestUserHttpClient.PostAsync(url, null);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}