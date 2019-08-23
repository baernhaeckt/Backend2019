using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Core.Security
{
    public static class SecurityKeyProvider
    {
        public static SymmetricSecurityKey GetSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authneticatio"));
    }
}
