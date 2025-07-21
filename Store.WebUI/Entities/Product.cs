namespace Store.WebUI.Entity
{
    public class Product : BaseEntity
    {
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public decimal DiscountAmount { get; set; }
        public List<string>? Badges { get; set; }
    }
}
