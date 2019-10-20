using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Persistence.Abstraction;
using Backend.Infrastructure.Security.Abstraction;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Queries
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
            _logger.UserSignInInitiated(query.Email);

            Tuple<Guid, string, string, IEnumerable<string>>? userByEmail = await _reader.SingleOrDefaultAsync<User, Tuple<Guid, string, string, IEnumerable<string>>>(
                u => u.Email == query.Email.ToLowerInvariant(),
                u => new Tuple<Guid, string, string, IEnumerable<string>>(u.Id, u.PasswordHash, u.Email, u.Roles));

            if (userByEmail == null)
            {
                _logger.UserSignInUserNotFound(query.Email);
                return new SignInQueryResult(true, false, string.Empty);
            }

            bool passwordOk = _passwordStorage.Match(query.Password, userByEmail.Item2);
            if (!passwordOk)
            {
                _logger.UserSignInPasswordMismatch(query.Email);
                return new SignInQueryResult(false, true, string.Empty);
            }

            _logger.UserSignInSuccessful(query.Email);
            string token = _securityTokenFactory.Create(userByEmail.Item1, userByEmail.Item3, userByEmail.Item4);
            return new SignInQueryResult(false, false, token);
        }
    }
}