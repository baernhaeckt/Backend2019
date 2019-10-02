using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.UserManagement.Security.Abstraction;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Queries
{
    internal class SecurityTokenForUserQueryHandler : ISubscriber
    {
        private readonly IReader _reader;

        private readonly ISecurityTokenFactory _securityTokenFactory;

        public SecurityTokenForUserQueryHandler(IReader reader, ISecurityTokenFactory securityTokenFactory)
        {
            _reader = reader;
            _securityTokenFactory = securityTokenFactory;
        }

        public async Task<SecurityTokenForUserQueryResult> ExecuteAsync(SecurityTokenForUserQuery query)
        {
            Tuple<Guid, string, IEnumerable<string>> result = await _reader.SingleAsync<User, Tuple<Guid, string, IEnumerable<string>>>(
                u => u.Email == query.Email.ToLowerInvariant(),
                u => new Tuple<Guid, string, IEnumerable<string>>(u.Id, u.Email, u.Roles));

            string token = _securityTokenFactory.Create(result.Item1, result.Item2, result.Item3);
            return new SecurityTokenForUserQueryResult(token);
        }
    }
}