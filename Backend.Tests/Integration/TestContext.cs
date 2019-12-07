using System.Net.Http;
using Backend.Infrastructure.Abstraction.Email;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Email.Fakes;
using Backend.Tests.Utilities;
using Backend.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Environments = Backend.Infrastructure.Abstraction.Hosting.Environments;

namespace Backend.Tests.Integration
{
    public class TestContext : WebApplicationFactory<Startup>
    {
        public TestContext()
        {
            NewTestUserHttpClient = CreateClient();
            AnonymousHttpClient = CreateClient();
        }

        public bool UseMongoDb { get; } = false;

        public InMemoryEmailService EmailService { get; } = new InMemoryEmailService(Substitute.For<ILogger<InMemoryEmailService>>());

        public string NewTestUser { get; set; } = string.Empty;

        public HttpClient NewTestUserHttpClient { get; }

        public HttpClient AnonymousHttpClient { get; }

        public HttpClient CreateNewHttpClient() => CreateClient();

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(Environments.IntegrationTest);

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