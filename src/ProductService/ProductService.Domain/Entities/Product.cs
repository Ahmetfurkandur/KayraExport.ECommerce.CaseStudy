namespace ProductService.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ProductCode { get; set; }
        public bool IsActive { get; set; } = true; // Soft delete için
        public int CategoryId { get; set; }
    }
}
