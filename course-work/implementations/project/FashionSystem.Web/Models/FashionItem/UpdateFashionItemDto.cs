using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FashionSystem.Web.Models.FashionItem
{
    public class UpdateFashionItemDto
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
        [Range(0.01, 100000)]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}