using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Security
{
    public interface IPasswordStorage
    {
        string Create(string password);

        bool Match(string inputPassword, string originalPassword);
    }
}
