using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Points.Models;
using Backend.Tests.Integration.Utilities.Extensions;
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
            var url = new Uri("api/points", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            IEnumerable<PointResponse> pointResponse = await response.OnSuccessDeserialize<IEnumerable<PointResponse>>();
            Assert.Equal(4, pointResponse.Count());
        }
    }
}