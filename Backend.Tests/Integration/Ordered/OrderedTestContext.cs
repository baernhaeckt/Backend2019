using System.Collections.Generic;
using System.Net.Http;
using Backend.Infrastructure.Email.Abstraction;
using Backend.Infrastructure.Email.Fakes;
using Backend.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Tests.Integration
{
    public class OrderedTestContext : WebApplicationFactory<Startup>
    {
        public OrderedTestContext()
        {
            AnonymousHttpClient = CreateClient();
            NewTestUserHttpClient = CreateClient();
            PartnerHttpClient = CreateClient();
        }

        public string NewTestUser { get; set; } = string.Empty;

        public HttpClient AnonymousHttpClient { get; }

        public HttpClient NewTestUserHttpClient { get; }

        public InMemoryEmailService EmailService { get; } = new InMemoryEmailService();

        public HttpClient PartnerHttpClient { get; }

        public IList<string> PartnerGeneratedTokens { get; } = new List<string>();

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(Environments.Development);

            builder.ConfigureServices(s => s.AddSingleton<IEmailService>(EmailService));

            return base.CreateHost(builder);
        }
    }
}