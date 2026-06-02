using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FashionSystem.Models.FashionItem
{
    public class CreateFashionItemDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Designer { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Style { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Size { get; set; } = string.Empty;

        [Required]
        public decimal PricePerDay { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        // OLD URL
        public string? ImageUrl { get; set; }

        // NEW FILE
        public IFormFile? ImageFile { get; set; }
    }
}