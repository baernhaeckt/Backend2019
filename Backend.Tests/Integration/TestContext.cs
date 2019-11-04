using System.Net.Http;
using Backend.Infrastructure.Email.Abstraction;
using Backend.Infrastructure.Email.Fakes;
using Backend.Infrastructure.Persistence.Abstraction;
using Backend.Tests.Integration.Utilities;
using Backend.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Backend.Tests.Integration
{
    public class TestContext : WebApplicationFactory<Startup>
    {
        private const bool UseMongoDb = false;

        public TestContext() => NewTestUserHttpClient = CreateClient();

        public InMemoryEmailService EmailService { get; } = new InMemoryEmailService(Substitute.For<ILogger<InMemoryEmailService>>());

        public string NewTestUser { get; set; } = string.Empty;

        public HttpClient NewTestUserHttpClient { get; }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Development);

            builder.ConfigureServices(s =>
            {
                s.AddSingleton<IEmailService>(EmailService);

                if (!UseMongoDb)
#pragma warning disable 162
                {
                    IUnitOfWork uow = new InMemoryUnitOfWork();
                    s.AddSingleton<IReader>(uow);
                    s.AddSingleton(uow);

                    s.AddSingleton<IIndexCreator, NullIndexCreator>();
                }
#pragma warning restore 162
            });

            return base.CreateHost(builder);
        }
    }
}