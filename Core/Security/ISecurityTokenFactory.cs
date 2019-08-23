using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
    interface ISecurityTokenFactory
    {
        string Create(User user);
    }
}
