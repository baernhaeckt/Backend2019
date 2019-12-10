using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.Partner.Models
{
    public class PartnerLoginRequest
    {
        [Required]
        public Guid PartnerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Secret { get; set; }
    }
}