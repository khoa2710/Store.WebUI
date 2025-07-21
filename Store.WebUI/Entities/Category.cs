namespace Store.WebUI.Entity
{
    public class Category  : BaseEntity
    {
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Icon { get; set; }
        public string? Slug { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
