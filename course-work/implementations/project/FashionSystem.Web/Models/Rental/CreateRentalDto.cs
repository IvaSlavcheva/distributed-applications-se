using System.ComponentModel.DataAnnotations;

namespace FashionSystem.Web.Models.Rental
{
    public class CreateRentalDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int FashionItemId { get; set; }

        [Required]
        public DateTime RentDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }
    }
}