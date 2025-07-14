using Store.WebUI.Models.Dto;

namespace Store.WebUI.Models.Dto
{
    public class OrderDto
    {
        //------------ORDER INFORMATION------------
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public int CustomerId { get; set; }
        public CustomerDto? Customer { get; set; }
        public string? OrderAddress { get; set; }
        public string? Status { get; set; }
        public string? BillingAddress { get; set; }
        public bool IsActive { get; set; }
        public string? Note { get; set; }

    


    }
}
