using System.ComponentModel.DataAnnotations;

namespace FashionSystem.Entities
{
    public class FashionItem
    {
        public int Id { get; set; }

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

        [MaxLength(1000)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public ICollection<Rental> Rentals { get; set; }
            = new List<Rental>();
    }
}