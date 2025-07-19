    namespace Store.WebUI.Entity
{
    public class Customer : BaseEntity
    {
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
