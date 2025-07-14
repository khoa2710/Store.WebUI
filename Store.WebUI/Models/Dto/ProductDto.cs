using Microsoft.Identity.Client;

namespace Store.WebUI.Models.Dto    
{
    public class ProductDto   
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<string>? Badges { get; set; }
        public int CategoryId   { get; set; }
        public string? CategoryName { get; set; }
    }
}
