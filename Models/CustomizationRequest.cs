using System.ComponentModel.DataAnnotations;

namespace NouvoStudio.Models
{
    public class CustomizationRequest
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[0-9]{0,10}$", ErrorMessage = "Phone must be digits only, up to 10.")]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ArtworkType { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Medium { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Size { get; set; } = string.Empty;
        
        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}

