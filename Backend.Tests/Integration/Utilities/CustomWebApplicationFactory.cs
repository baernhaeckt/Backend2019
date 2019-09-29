using Backend.Infrastructure.Email.Abstraction;
using Backend.Tests.Integration.Fakes;
using Backend.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Tests.Integration.Utilities
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public InMemoryEmailService EmailService { get; } = new InMemoryEmailService();

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Development);

            builder.ConfigureServices(s => s.AddSingleton<IEmailService>(EmailService));

            return base.CreateHost(builder);
        }
    }
}