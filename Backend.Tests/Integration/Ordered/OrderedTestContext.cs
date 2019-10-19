using System.Collections.Generic;
using System.Net.Http;

namespace Backend.Tests.Integration
{
    public class OrderedTestContext : TestContext
    {
        public OrderedTestContext()
        {
            AnonymousHttpClient = CreateClient();
            PartnerHttpClient = CreateClient();
        }

        public HttpClient AnonymousHttpClient { get; }

        public HttpClient PartnerHttpClient { get; }

        public IList<string> PartnerGeneratedTokens { get; } = new List<string>();
    }
}