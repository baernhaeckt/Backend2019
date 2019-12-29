using System.Collections.Generic;
using System.Linq;

namespace Backend.Core.Entities.Partner
{
    public class TokenIssuer : Entity
    {
        public string Name { get; set; } = string.Empty;

        public string SecretHash { get; set; } = string.Empty;

        public IList<Token> PrototypeTokens { get; set; } = Enumerable.Empty<Token>().ToList();
    }
}