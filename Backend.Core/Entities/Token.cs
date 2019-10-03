using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Entities
{
    public class Token : Entity
    {
        public string Text { get; set; } = string.Empty;

        public int Points { get; set; }

        public double Co2Saving { get; set; }

        public Guid PartnerId { get; set; }

        public SufficientType SufficientType { get; set; } = new SufficientType();

        public string Name { get; set; } = string.Empty;

        public string TokenType { get; set; } = string.Empty;

        public bool IsSingleUse { get; set; } = true;

        public IList<Guid> UsedBy { get; set; } = Enumerable.Empty<Guid>().ToList();
    }
}