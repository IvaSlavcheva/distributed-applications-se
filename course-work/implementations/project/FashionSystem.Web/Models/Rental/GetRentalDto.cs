namespace FashionSystem.Web.Models.Rental
{
    public class GetRentalDto
    {
        public int Id { get; set; }

        public int FashionItemId { get; set; }

        public string FashionItemName { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public DateTime RentDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}