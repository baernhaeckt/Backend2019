using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Tests.Integration.Utilities;
using Xunit;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class UserManagementFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public UserManagementFixture(CustomWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task GetProfileControllerProfile_NormalUser_Ok()
        {
            HttpClient client = _factory.CreateClient();

            string email = DataGenerator.RandomEmail();
            var url = new Uri("api/users/Register?email=" + email, UriKind.Relative);
            HttpResponseMessage response = await client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();

            Assert.Single(_factory.EmailService.Messages);
            Assert.Equal(email, _factory.EmailService.Messages.Single().Receiver);
        }
    }
}