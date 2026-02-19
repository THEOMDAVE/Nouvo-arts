using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NouvoStudio.Models
{
    public class Artwork
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Size { get; set; } = string.Empty;
        
        public decimal HeightFeet { get; set; }
        
        public decimal WidthFeet { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Medium { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Image { get; set; } = string.Empty;
        
        public bool IsFeatured { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Category IDs as comma-separated string
        [StringLength(500)]
        public string CategoryIds { get; set; } = string.Empty;

        [StringLength(500)]
        public string SpaceIds { get; set; } = string.Empty;
    }


    public class ArtworkListViewModel
    {
        public IEnumerable<Artwork> Artworks { get; set; } = new List<Artwork>();

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public string? SearchQuery { get; set; }
        public string? SelectedSize { get; set; }
        public string? SelectedMedium { get; set; }

        public int? SelectedCategoryId { get; set; }
        public int? SelectedSpaceId { get; set; }

        public string? Name { get; set; }

        public IEnumerable<Medium> Mediums { get; set; } = new List<Medium>();
    }


}
