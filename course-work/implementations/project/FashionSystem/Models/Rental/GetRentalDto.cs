namespace FashionSystem.Models.Rental
{
    public class GetRentalDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FashionItemId { get; set; }

        public DateTime RentDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
