using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework.Cqrs;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement.Queries
{
    internal class SecurityTokenForUserQueryHandler : QueryHandler<SecurityTokenForUserQueryResult, SecurityTokenForUserQuery>
    {
        private readonly ISecurityTokenFactory _securityTokenFactory;

        public SecurityTokenForUserQueryHandler(IReader reader, ILogger<SecurityTokenForUserQueryHandler> logger, ISecurityTokenFactory securityTokenFactory)
            : base(reader, logger) => _securityTokenFactory = securityTokenFactory;

        public override async Task<SecurityTokenForUserQueryResult> ExecuteAsync(SecurityTokenForUserQuery query)
        {
            Logger.RetrieveSecurityTokenForUser(query.Email);

            Tuple<Guid, string, IEnumerable<string>> result = await Reader.SingleAsync<User, Tuple<Guid, string, IEnumerable<string>>>(
                u => u.Email == query.Email.ToLowerInvariant(),
                u => new Tuple<Guid, string, IEnumerable<string>>(u.Id, u.Email, u.Roles));

            string token = _securityTokenFactory.Create(result.Item1, result.Item2, result.Item3);

            Logger.RetrieveSecurityTokenForUserSuccessful(query.Email, result.Item1);

            return new SecurityTokenForUserQueryResult(token);
        }
    }
}