using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Core.Entities.Partner
{
    public class Token : Entity, ICloneable
    {
        public LocalizedField Text { get; set; } = LocalizedField.Empty;

        public int Points { get; set; }

        public double Co2Saving { get; set; }

        public Guid PartnerId { get; set; }

        public SufficientType SufficientType { get; set; }

        public string TokenType { get; set; } = string.Empty;

        public bool IsSingleUse { get; set; } = true;

        public IList<Guid> UsedBy { get; set; } = Enumerable.Empty<Guid>().ToList();

        public object Clone() => MemberwiseClone();

        public Token CreateFromPrototype()
        {
            var newToken = (Token)MemberwiseClone();
            newToken.Id = Guid.Empty;
            newToken.PartnerId = Guid.Empty;
            newToken.UsedBy = Enumerable.Empty<Guid>().ToList();
            return newToken;
        }
    }
}