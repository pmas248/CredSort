using System.ComponentModel.DataAnnotations;
using System.Data;

namespace CredSort.Api.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        [Required]
        public Guid TenantId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
