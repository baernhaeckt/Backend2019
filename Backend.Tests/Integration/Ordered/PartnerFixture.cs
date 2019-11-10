﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Partner.Data.Testing;
using Backend.Core.Features.UserManagement.Models;
using Backend.Tests.Integration.Utilities.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Backend.Tests.Integration
{
    [Order(3)]
    [Collection("IntegrationTests")]
    [Trait("Category", "Integration")]
    public class PartnerFixture
    {
        private readonly OrderedTestContext _context;

        public PartnerFixture(OrderedTestContext context) => _context = context;

        [Order(0)]
        [Fact]
        public async Task TokenIssuersLogin_Successful()
        {
            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id2, TokenIssuerTestCredentials.Secret2);
        }

        [Order(1)]
        [Fact]
        public async Task TokensCreate_ValidPartnerId_Ok()
        {
            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id1, TokenIssuerTestCredentials.Secret1);
            var url = new Uri("api/tokens?tokenType=VerpAckUng", UriKind.Relative);

            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string tokenValue = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
            _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());

            response = await _context.PartnerHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            tokenValue = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
            _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());

            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id3, TokenIssuerTestCredentials.Secret3);
            url = new Uri("api/tokens?tokenType=TeIleN", UriKind.Relative);

            response = await _context.PartnerHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            tokenValue = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
            _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());

            url = new Uri("api/tokens?tokenType=multiuse", UriKind.Relative);
            response = await _context.PartnerHttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            tokenValue = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrWhiteSpace(tokenValue));
            _context.PartnerGeneratedTokens.Add(await response.Content.ReadAsStringAsync());
        }

        [Order(1)]
        [Fact]
        public async Task GetToken_InvalidTokenType_NOk()
        {
            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(new Uri("api/tokens?tokenType=bla", UriKind.Relative));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Order(2)]
        public async Task TokensUse_Successful()
        {
            foreach (string tokenValue in _context.PartnerGeneratedTokens)
            {
                var url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
                HttpResponseMessage response = await _context.NewTestUserHttpClient.PostAsync(url, null);
                response.EnsureSuccessStatusCode();
            }
        }

        [Order(3)]
        [Fact]
        public async Task UseToken_SingleUseAlreadyUsed_NOk()
        {
            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id3, TokenIssuerTestCredentials.Secret3);
            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(new Uri("api/tokens?tokenType=TeIleN", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            string tokenValue = await response.Content.ReadAsStringAsync();

            var url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
            response = await _context.NewTestUserHttpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();

            response = await _context.NewTestUserHttpClient.PostAsync(url, null);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Order(4)]
        [Fact]
        public async Task UseToken_MultiUseAlreadyUsed_Ok()
        {
            await _context.PartnerHttpClient.SignInTokenIssuer(TokenIssuerTestCredentials.Id3, TokenIssuerTestCredentials.Secret3);
            HttpResponseMessage response = await _context.PartnerHttpClient.GetAsync(new Uri("api/tokens?tokenType=multiuse", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            string tokenValue = await response.Content.ReadAsStringAsync();

            for (int i = 0; i < 2; i++)
            {
                await UseMultiuseToken(_context.NewTestUserHttpClient, tokenValue);
            }

            // Will be used to test the ranking.
            // Prepare some users and use tokens to get them points.
            // One of them hasn't the same Zip and one is not friend.
            for (int i = 0; i < 5; i++)
            {
                await CreateUserMakeFriendWithTestUserAndUseToken("4000", tokenValue, i);
            }

            // This user shouldn't be in the "local" ranking later.
            await CreateUserMakeFriendWithTestUserAndUseToken("4001", tokenValue, 5);
        }

        private async Task CreateUserMakeFriendWithTestUserAndUseToken(string zip, string tokenValue, int numberOfTokenUse)
        {
            var newHttpClient = _context.CreateNewHttpClient();
            string newUserEmail = await newHttpClient.CreateUserAndSignIn();

            var url = new Uri("api/profile", UriKind.Relative);
            StringContent content = new UpdateProfileModel { DisplayName = "abc1", Street = "Abc", City = "Abc", PostalCode = zip }.ToStringContent();
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PatchAsync(url, content);
            response.EnsureSuccessStatusCode();

            for (int i = 0; i < numberOfTokenUse; i++)
            {
                await UseMultiuseToken(newHttpClient, tokenValue);
            }

            response = await _context.NewTestUserHttpClient.PostAsync(new Uri($"api/friends?friendEmail={newUserEmail}", UriKind.Relative), null);
            response.EnsureSuccessStatusCode();
        }

        private static async Task UseMultiuseToken(HttpClient client, string tokenValue)
        {
            var url = new Uri("api/tokens?tokenGuid=" + tokenValue, UriKind.Relative);
            HttpResponseMessage response = await client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
        }
    }
}