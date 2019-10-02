using System.Net.Http;
using Backend.Infrastructure.Email.Abstraction;
using Backend.Infrastructure.Email.Fakes;
using Backend.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Tests.Integration
{
    public class TestContext : WebApplicationFactory<Startup>
    {
        public TestContext() => NewUserHttpClient = CreateClient();

        public InMemoryEmailService EmailService { get; } = new InMemoryEmailService();

        public string NewTestUser { get; set; } = string.Empty;

        public HttpClient NewUserHttpClient { get; }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Development);

            builder.ConfigureServices(s => s.AddSingleton<IEmailService>(EmailService));

            return base.CreateHost(builder);
        }
    }
}