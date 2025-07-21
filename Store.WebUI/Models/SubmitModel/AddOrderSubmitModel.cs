using Store.WebUI.Entity;
using System.ComponentModel.DataAnnotations;

namespace Store.WebUI.Models.SubmitModel
{
    public class AddOrderSubmitModel
    {
        [Required]
        [Display(Name = "Order Name")]
        public string? Name { get; set; }
        [Display(Name = "Order Date")]
        [Required]
        public DateTimeOffset OrderDate { get; set; }
        [Display(Name = "Customer ID under EMAIL")]
        [Required]
        public int CustomerId { get; set; }
        [Display(Name = "Address")]
        [Required]
        public string? OrderAddress { get; set; }
        [Display(Name = "Billing Address")]
        [Required]
        public string? BillingAddress { get; set; }
        public string? Status { get; set; }
        [Display(Name = "Activation Status")]
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string? Note { get; set; }


        //----Order Detail:-----------
        public int Quantity { get; set; }
        [Display(Name = "Product Id")]
        [Required]
        public int ProductId { get; set; }
        [Display(Name = "Order Id")]
        [Required]
        public int OrderId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset EditAt { get; set; }
    }
}
