using System;
using System.Collections.Generic;

namespace Backend.Core.Features.UserManagement.Security.Abstraction
{
    public interface ISecurityTokenFactory
    {
        string Create(Guid id, string email, IEnumerable<string> roles);
    }
}