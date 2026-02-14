using System.ComponentModel.DataAnnotations;

namespace NouvoStudio.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(20)]
        [RegularExpression(@"^[0-9]{0,10}$", ErrorMessage = "Phone must be digits only, up to 10.")]
        public string? Phone { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        
        // Comma-separated artwork codes the user is interested in
        public string? InterestedCodes { get; set; }
    }
}
