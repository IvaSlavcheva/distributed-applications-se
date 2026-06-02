namespace FashionSystem.Web.Models.FashionItem
{
    public class GetFashionItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Designer { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Style { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
