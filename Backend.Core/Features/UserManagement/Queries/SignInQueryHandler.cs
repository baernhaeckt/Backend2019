using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Core.Framework.Cqrs;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement.Queries
{
    internal class SignInQueryHandler : QueryHandler<SignInQueryResult, SignInQuery>
    {
        private readonly IPasswordStorage _passwordStorage;

        private readonly ISecurityTokenFactory _securityTokenFactory;

        public SignInQueryHandler(IReader reader, ILogger<SignInQueryHandler> logger, IPasswordStorage passwordStorage, ISecurityTokenFactory securityTokenFactory)
            : base(reader, logger)
        {
            _passwordStorage = passwordStorage;
            _securityTokenFactory = securityTokenFactory;
        }

        public override async Task<SignInQueryResult> ExecuteAsync(SignInQuery query)
        {
            Logger.ExecuteUserSignIn(query.Email);

            Tuple<Guid, string, string, IEnumerable<string>>? userByEmail = await Reader.SingleOrDefaultAsync<User, Tuple<Guid, string, string, IEnumerable<string>>>(
                u => u.Email == query.Email.ToLowerInvariant(),
                u => new Tuple<Guid, string, string, IEnumerable<string>>(u.Id, u.PasswordHash, u.Email, u.Roles));

            if (userByEmail == null)
            {
                Logger.UserSignInUserNotFound(query.Email);
                return new SignInQueryResult(true, false, string.Empty);
            }

            bool passwordOk = _passwordStorage.Match(query.Password, userByEmail.Item2);
            if (!passwordOk)
            {
                Logger.UserSignInPasswordMismatch(query.Email);
                return new SignInQueryResult(false, true, string.Empty);
            }

            string token = _securityTokenFactory.Create(userByEmail.Item1, userByEmail.Item3, userByEmail.Item4);
            var result = new SignInQueryResult(false, false, token);

            Logger.ExecuteUserSignInSuccessful(query.Email);
            return result;
        }
    }
}