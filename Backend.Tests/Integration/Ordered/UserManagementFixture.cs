using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Models;
using Backend.Core.Features.UserManagement.Queries;
using Backend.Tests.Utilities;
using Backend.Tests.Utilities.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace Backend.Tests.Integration
{
    [Order(1)]
    [Collection("IntegrationTests")]
    [Trait("Category", "Integration")]
    public class UserManagementFixture
    {
        private readonly OrderedTestContext _context;

        private readonly ITestOutputHelper _output;

        public UserManagementFixture(OrderedTestContext context, ITestOutputHelper output)
        {
            _context = context;
            _output = output;
        }

        [Fact]
        [Order(0)]
        public async Task UsersRegister_SuccessfulAndSendsEmail()
        {
            string newUserEmail = DataGenerator.RandomEmail();
            _output.WriteLine("Register new user: " + newUserEmail);

            _context.NewTestUser = newUserEmail;

            var url = new Uri("api/users/Register", UriKind.Relative);
            StringContent content = new RegisterUserRequest { Email = newUserEmail }.ToStringContent();
            HttpResponseMessage response = await _context.AnonymousHttpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            Assert.Single(_context.EmailService.Messages);
            Assert.Equal(newUserEmail.ToLowerInvariant(), _context.EmailService.Messages.Single().Receiver);
        }

        [Fact]
        [Order(1)]
        public async Task UsersLogin_Successful()
        {
            _output.WriteLine("Sign in with the user: " + _context.NewTestUser);

            await _context.NewTestUserHttpClient.SignIn(_context.NewTestUser, "1234");
        }

        [Fact]
        [Order(2)]
        public async Task Profile_RetrieveSuccessful()
        {
            _output.WriteLine("Get profile of user: " + _context.NewTestUser);
            var url = new Uri("api/profile", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(url);
            UserProfileQueryResult userResponse = await response.OnSuccessDeserialize<UserProfileQueryResult>();
            Assert.Equal(_context.NewTestUser.ToLowerInvariant(), userResponse.Email);
        }

        [Fact]
        [Order(3)]
        public async Task Profile_UpdateSuccessful()
        {
            _output.WriteLine("Update profile of user: " + _context.NewTestUser);
            var url = new Uri("api/profile", UriKind.Relative);
            StringContent content = new UpdateProfileRequest { DisplayName = "abc", Street = "Abc", City = "Abc", PostalCode = "3032" }.ToStringContent();
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PatchAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        [Order(4)]
        public async Task ProfilePasswordChange_Successful()
        {
            _output.WriteLine("Change password of user: " + _context.NewTestUser);
            StringContent content = new ChangePasswordRequest { OldPassword = "1234", NewPassword = "12345678" }.ToStringContent();
            var url = new Uri("api/profile/password", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PatchAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        [Order(5)]
        public async Task UsersLogin_WrongPassword_NOk()
        {
            var url = new Uri("api/users/Login", UriKind.Relative);
            StringContent content = new UserLoginRequest { Email = _context.NewTestUser, Password = "12345" }.ToStringContent();
            HttpResponseMessage responseWithJwt = await _context.AnonymousHttpClient.PostAsync(url, content);
            Assert.Equal(HttpStatusCode.Forbidden, responseWithJwt.StatusCode);
        }

        [Fact]
        [Order(6)]
        public async Task UsersLogin_WrongUsername_NOk()
        {
            var url = new Uri("api/users/Login", UriKind.Relative);
            StringContent content = new UserLoginRequest { Email = "bla@bla.ch", Password = "12345" }.ToStringContent();
            HttpResponseMessage responseWithJwt = await _context.AnonymousHttpClient.PostAsync(url, content);
            Assert.Equal(HttpStatusCode.NotFound, responseWithJwt.StatusCode);
        }
    }
}