using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Features.UserManagement.Models
{
    public class UpdateProfileModel
    {
        [Required]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;
    }
}