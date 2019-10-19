using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Models;
using Backend.Tests.Integration.Utilities.Extensions;
using Xunit;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class ProfileControllerFixture : IClassFixture<TestContext>
    {
        private readonly TestContext _context;

        public ProfileControllerFixture(TestContext context) => _context = context;

        [Fact]
        public async Task UpdateProfile_Successful()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();

            var uri = new Uri("api/profile", UriKind.Relative);
            StringContent content = new UpdateProfileModel
            {
                DisplayName = "test1234",
                PostalCode = "3032",
                Street = "Kappelenring 33b",
                City = "Hinterkappelen"
            }.ToStringContent();
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PatchAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetProfile_Successful()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();

            var uri = new Uri("api/profile", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ChangePassword_Successful()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();

            var uri = new Uri("api/profile/password", UriKind.Relative);
            StringContent content = new ChangePasswordModel { OldPassword = "1234", NewPassword = "12345678" }.ToStringContent();
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PatchAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ChangePassword_OldPasswordInvalid_BadRequest()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();

            var uri = new Uri("api/profile/password", UriKind.Relative);
            StringContent content = new ChangePasswordModel { OldPassword = "12345", NewPassword = "12345678" }.ToStringContent();
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PatchAsync(uri, content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_NewPasswordTooShort_BadRequest()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();

            var uri = new Uri("api/profile/password", UriKind.Relative);
            StringContent content = new ChangePasswordModel { OldPassword = "1234", NewPassword = "1234567" }.ToStringContent();
            HttpResponseMessage response = await _context.NewTestUserHttpClient.PatchAsync(uri, content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}