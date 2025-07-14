using Store.WebUI.Entity;
using System.ComponentModel.DataAnnotations;

namespace Store.WebUI.Models.SubmitModel
{
    public class AddOrderSubmitModel
    {
        [Display(Name = "Order Name")]
        public string? Name { get; set; }
        [Display(Name = "Order Date")]
        public DateTimeOffset OrderDate { get; set; }
        [Display(Name = "Customer ID under EMAIL")]
        public int CustomerId { get; set; }
        [Display(Name = "Address")]
        public string? OrderAddress { get; set; }
        [Display(Name = "Billing Address")]
        public string? BillingAddress { get; set; }
        public string? Status { get; set; }
        [Display(Name = "Activation Status")]
        public bool IsActive { get; set; }
        public string? Note { get; set; }


        //----Order Detail:-----------
        public int Quantity { get; set; }
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset EditAt { get; set; }
    }
}
