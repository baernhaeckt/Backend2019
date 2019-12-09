using System;

namespace Backend.Core.Features.Partner.Models
{
    public class PartnerLoginRequest
    {
        public Guid PartnerId { get; set; }

        public string Secret { get; set; }
    }
}