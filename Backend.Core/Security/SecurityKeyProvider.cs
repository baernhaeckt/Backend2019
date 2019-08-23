using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Core.Security
{
    public static class SecurityKeyProvider
    {
        public static SymmetricSecurityKey GetSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234"));
    }
}
