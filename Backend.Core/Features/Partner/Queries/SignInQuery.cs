using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Partner.Queries
{
    public class SignInQuery : IQuery<SignInQueryResult>
    {
        public SignInQuery(Guid id, string secret)
        {
            Id = id;
            Secret = secret;
        }

        public Guid Id { get; }

        public string Secret { get; }
    }
}