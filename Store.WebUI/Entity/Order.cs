namespace Store.WebUI.Entity
{
    public class Order : BaseEntity
    {
        public DateTimeOffset OrderDate { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string? OrderAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; }
        public string? Note { get; set; }
        //fk
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
    }
}
