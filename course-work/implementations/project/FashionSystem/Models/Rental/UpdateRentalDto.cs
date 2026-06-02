using System.ComponentModel.DataAnnotations;

namespace FashionSystem.Models.Rental
{
    public class UpdateRentalDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int FashionItemId { get; set; }

        [Required]
        public DateTime RentDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;
    }
}
