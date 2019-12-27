using System;
using System.ComponentModel.DataAnnotations;
using Backend.Core.Framework.Web;

namespace Backend.Core.Features.Partner.Models
{
    public class PartnerLoginRequest
    {
        [Required]
        [NotEmpty]
        public Guid PartnerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Secret { get; set; }
    }
}