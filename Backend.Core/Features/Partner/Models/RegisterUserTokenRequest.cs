using System;
using Backend.Core.Framework.Web;

namespace Backend.Core.Features.Partner.Models
{
    public class RegisterUserTokenRequest
    {
        [NotEmpty]
        public Guid TokenId { get; set; }
    }
}