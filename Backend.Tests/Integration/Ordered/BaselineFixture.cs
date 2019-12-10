using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.Baseline.Queries;
using Backend.Tests.Utilities.Extensions;
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
            Task<IEnumerable<SufficientTypesQueryResult>> result = response.OnSuccessDeserialize<IEnumerable<SufficientTypesQueryResult>>();
        }

        [Fact]
        public async Task SufficientTypeUser_Successful()
        {
            var url = new Uri("api/SufficientType/user", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            List<SufficientTypesQueryResult> result = (await response.OnSuccessDeserialize<IEnumerable<SufficientTypesQueryResult>>()).ToList();
            Assert.Equal(30, result.Single(r => r.Type == SufficientType.FoodWaste).Points);
            Assert.Equal(3, result.Single(r => r.Type == SufficientType.FoodWaste).Co2Savings);
            Assert.Equal(30, result.Single(r => r.Type == SufficientType.Share).Points);
            Assert.Equal(4, result.Single(r => r.Type == SufficientType.Share).Co2Savings);
            Assert.Equal(20, result.Single(r => r.Type == SufficientType.Packing).Points);
            Assert.Equal(2, result.Single(r => r.Type == SufficientType.Packing).Co2Savings);
        }
    }
}