using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Entities
{
    public class TokenIssuer : Entity
    {
        public string Name { get; set; } = string.Empty;

        public string SecretHash { get; set; } = string.Empty;

        public IList<Token> PrototypeTokens { get; set; } = Enumerable.Empty<Token>().ToList();

        public IList<Guid> Users { get; set; } = Enumerable.Empty<Guid>().ToList();
    }
}