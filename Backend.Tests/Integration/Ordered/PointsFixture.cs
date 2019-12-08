using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Points.Models;
using Backend.Core.Features.Points.Queries;
using Backend.Tests.Utilities.Extensions;
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
            IEnumerable<PointHistoryForUserQueryResult> pointResponse = await response.OnSuccessDeserialize<IEnumerable<PointHistoryForUserQueryResult>>();
            Assert.Equal(7, pointResponse.Count());
        }

        [Fact]
        public async Task RankingGlobal_Successful()
        {
            var url = new Uri("api/rankings/global", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RankingLocal_Successful()
        {
            var url = new Uri("api/rankings/local?zip=3008", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RankingFriends_Successful()
        {
            var url = new Uri("api/rankings/friends", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RankingSummary_Successful()
        {
            var url = new Uri("api/rankings/summary", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}