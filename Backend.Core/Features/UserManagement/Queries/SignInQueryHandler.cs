using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Persistence.Abstraction;
using Backend.Infrastructure.Security.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Queries
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
            Tuple<Guid, string, string, IEnumerable<string>>? userByEmail = await _reader.SingleOrDefaultAsync<User, Tuple<Guid, string, string, IEnumerable<string>>>(
                u => u.Email == query.Email.ToLowerInvariant(),
                u => new Tuple<Guid, string, string, IEnumerable<string>>(u.Id, u.PasswordHash, u.Email, u.Roles));

            if (userByEmail == null)
            {
                return new SignInQueryResult(true, false, string.Empty);
            }

            bool passwordOk = _passwordStorage.Match(query.Password, userByEmail.Item2);
            if (!passwordOk)
            {
                return new SignInQueryResult(false, true, string.Empty);
            }

            string token = _securityTokenFactory.Create(userByEmail.Item1, userByEmail.Item3, userByEmail.Item4);
            return new SignInQueryResult(false, false, token);
        }
    }
}