using System;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Persistence.Abstraction;
using Backend.Infrastructure.Security.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Partner.Queries
{
    internal class SignInQueryHandler : ISubscriber
    {
        private readonly IPasswordStorage _passwordStorage;

        private readonly IReader _reader;

        private readonly ISecurityTokenFactory _securityTokenFactory;

        public SignInQueryHandler(IReader reader, IPasswordStorage passwordStorage, ISecurityTokenFactory securityTokenFactory)
        {
            _reader = reader;
            _passwordStorage = passwordStorage;
            _securityTokenFactory = securityTokenFactory;
        }

        public async Task<SignInQueryResult> ExecuteAsync(SignInQuery query)
        {
            Tuple<Guid, string, string> result = await _reader.GetByIdOrThrowAsync<TokenIssuer, Tuple<Guid, string, string>>(
                query.Id, u => new Tuple<Guid, string, string>(u.Id, u.Name, u.SecretHash));

            if (result == null)
            {
                return new SignInQueryResult(true, false, string.Empty);
            }

            bool passwordOk = _passwordStorage.Match(query.Secret, result.Item3);
            if (!passwordOk)
            {
                return new SignInQueryResult(false, true, string.Empty);
            }

            string token = _securityTokenFactory.Create(result.Item1, result.Item2, new[] { Roles.Partner });
            return new SignInQueryResult(false, false, token);
        }
    }
}