using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Friendship.Models;
using Backend.Core.Features.UserManagement.Data.Testing;
using Backend.Tests.Integration.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class FriendshipFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        private readonly ITestOutputHelper _output;

        public FriendshipFixture(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _output = testOutputHelper;
        }

        [Fact]
        public async Task Execute()
        {
            HttpClient client = _factory.CreateClient();

            string email = await client.CreateUserAndSignIn();
            _output.WriteLine("Sign in with the user " + email);

            // Initial the user hasn't any friends
            HttpResponseMessage response = await client.GetAsync(new Uri("api/friends", UriKind.Relative));
            List<FriendResponse> friendResponse = await response.OnSuccessDeserialize<List<FriendResponse>>();
            Assert.Empty(friendResponse);

            // Add two friends. After adding, the user has two friends
            response = await client.PostAsync(new Uri($"api/friends?friendEmail={TestCredentials.User2}", UriKind.Relative), null);
            response.EnsureSuccessStatusCode();
            response = await client.PostAsync(new Uri($"api/friends?friendEmail={TestCredentials.User3}", UriKind.Relative), null);
            response.EnsureSuccessStatusCode();
            response = await client.GetAsync(new Uri("api/friends", UriKind.Relative));
            friendResponse = await response.OnSuccessDeserialize<List<FriendResponse>>();
            Assert.Equal(2, friendResponse.Count);

            // Can't add the friend again
            response = await client.PostAsync(new Uri($"api/friends?friendEmail={TestCredentials.User2}", UriKind.Relative), null);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Remove a friend. One is left.
            string id = friendResponse.First().Id.ToString();
            response = await client.DeleteAsync(new Uri("api/friends?friendUserId=" + id, UriKind.Relative));
            response.EnsureSuccessStatusCode();
            response = await client.GetAsync(new Uri("api/friends", UriKind.Relative));
            friendResponse = await response.OnSuccessDeserialize<List<FriendResponse>>();
            Assert.Single(friendResponse);

            // Remove the second, none is left.
            id = friendResponse.First().Id.ToString();
            response = await client.DeleteAsync(new Uri("api/friends?friendUserId=" + id, UriKind.Relative));
            response.EnsureSuccessStatusCode();
            response = await client.GetAsync(new Uri("api/friends", UriKind.Relative));
            friendResponse = await response.OnSuccessDeserialize<List<FriendResponse>>();
            Assert.Empty(friendResponse);
        }
    }
}