using System;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Partner.Queries
{
    internal class SignInQueryHandler : ISubscriber
    {
        private readonly IPasswordStorage _passwordStorage;

        private readonly IReader _reader;

        private readonly ISecurityTokenFactory _securityTokenFactory;

        private readonly ILogger<SignInQueryHandler> _logger;

        public SignInQueryHandler(IReader reader, IPasswordStorage passwordStorage, ISecurityTokenFactory securityTokenFactory, ILogger<SignInQueryHandler> logger)
        {
            _reader = reader;
            _passwordStorage = passwordStorage;
            _securityTokenFactory = securityTokenFactory;
            _logger = logger;
        }

        public async Task<SignInQueryResult> ExecuteAsync(SignInQuery query)
        {
            _logger.PartnerSignInInitiated(query.Id);

            (Guid id, string? name, string? hash) = await _reader.GetByIdOrThrowAsync<TokenIssuer, Tuple<Guid, string, string>>(
                query.Id, u => new Tuple<Guid, string, string>(u.Id, u.Name, u.SecretHash));

            if (id == Guid.Empty)
            {
                _logger.PartnerSignInPartnerNotFound(query.Id);
                return new SignInQueryResult(true, false, string.Empty);
            }

            bool passwordOk = _passwordStorage.Match(query.Secret, hash);
            if (!passwordOk)
            {
                _logger.PartnerSignInPasswordMismatch(query.Id);
                return new SignInQueryResult(false, true, string.Empty);
            }

            _logger.PartnerSignInSuccessful(query.Id, name);
            string token = _securityTokenFactory.Create(id, name, new[] { Roles.Partner });
            return new SignInQueryResult(false, false, token);
        }
    }
}