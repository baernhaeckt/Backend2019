using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.UserManagement.Models
{
    public class UpdateProfileRequest
    {
        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [MaxLength(4)]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;
    }
}