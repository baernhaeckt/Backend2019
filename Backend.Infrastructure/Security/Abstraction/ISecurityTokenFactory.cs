using System;
using System.Collections.Generic;

namespace Backend.Infrastructure.Security.Abstraction
{
    public interface ISecurityTokenFactory
    {
        string Create(Guid id, string subject, IEnumerable<string> roles);
    }
}