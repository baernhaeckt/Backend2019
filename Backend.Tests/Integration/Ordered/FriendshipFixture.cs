using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Friendship.Models;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Tests.Integration.Utilities.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Backend.Tests.Integration
{
    [Order(3)]
    [Collection("IntegrationTests")]
    [Trait("Category", "Integration")]
    public class FriendshipFixture
    {
        private readonly OrderedTestContext _context;

        public FriendshipFixture(OrderedTestContext context) => _context = context;

        [Order(0)]
        [Fact]
        public async Task FriendsList_IsEmptyAtBeginning()
        {
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(new Uri("api/friends", UriKind.Relative));
            List<FriendResponse> friendResponse = await response.OnSuccessDeserialize<List<FriendResponse>>();
            Assert.Empty(friendResponse);
        }

        [Order(1)]
        [Fact]
        public async Task FriendsAdd_Successful()
        {
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PostAsync(new Uri($"api/friends?friendEmail={TestCredentials.User2}", UriKind.Relative), null);
            response.EnsureSuccessStatusCode();

            response = await _context.NewTestUserHttpClient.PostAsync(new Uri($"api/friends?friendEmail={TestCredentials.User3}", UriKind.Relative), null);
            response.EnsureSuccessStatusCode();

            response = await _context.NewTestUserHttpClient.GetAsync(new Uri("api/friends", UriKind.Relative));
            List<FriendResponse> friendResponse = await response.OnSuccessDeserialize<List<FriendResponse>>();
            Assert.Equal(2, friendResponse.Count);
        }

        [Order(2)]
        [Fact]
        public async Task FriendsAddTwice_NotSuccessful()
        {
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PostAsync(new Uri($"api/friends?friendEmail={TestCredentials.User2}", UriKind.Relative), null);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Order(3)]
        [Fact]
        public async Task FriendsDelete_Successful()
        {
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(new Uri("api/friends", UriKind.Relative));
            List<FriendResponse> friendResponse = await response.OnSuccessDeserialize<List<FriendResponse>>();

            // Remove a friend. One is left.
            string id = friendResponse.First().Id.ToString();
            response = await _context.NewTestUserHttpClient.DeleteAsync(new Uri("api/friends?friendUserId=" + id, UriKind.Relative));
            response.EnsureSuccessStatusCode();

            response = await _context.NewTestUserHttpClient.GetAsync(new Uri("api/friends", UriKind.Relative));
            friendResponse = await response.OnSuccessDeserialize<List<FriendResponse>>();
            Assert.Single(friendResponse);
        }
    }
}