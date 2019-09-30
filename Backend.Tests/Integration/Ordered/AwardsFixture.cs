using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Awards.Models;
using Backend.Tests.Integration.Utilities.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Backend.Tests.Integration
{
    [Order(5)]
    [Collection("IntegrationTests")]
    [Trait("Category", "Integration")]
    public class AwardsFixture
    {
        private readonly OrderedTestContext _context;

        public AwardsFixture(OrderedTestContext context) => _context = context;

        [Fact]
        public async Task AwardsRetrieve_Successful()
        {
            var url = new Uri("api/awards", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            List<AwardsResponse> awards = await response.OnSuccessDeserialize<List<AwardsResponse>>();
            Assert.Equal(2, awards.Count);
        }
    }
}