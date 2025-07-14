using Store.WebUI.Entity;
using System.ComponentModel.DataAnnotations;

namespace Store.WebUI.Models.SubmitModel
{
    public class AddProductSubmitModel
    {
        [Display(Name = "Ten san pham")]
        [Required(ErrorMessage = "Vui long nhap {0}")]
        [StringLength(255)]
        public string? Name { get; set; }
        [Display(Name = "TMo Ta")]

        public string? Description { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Link Hinh Anh")]
        public string? ImageUrl { get; set; }
        [Display(Name = "So luong trong kho")]
        public int StockQuantity { get; set; }

        [Display(Name = "Loai danh muc")]
        
        [Range(1, int.MaxValue, ErrorMessage = "Please choose a category")]
        public int CategoryId { get; set; }
        [Display(Name = "Trang Thai Hoat Dong")]
        public bool IsActive { get; set; }
        [Display(Name = "Discount")]
        public decimal DiscountAmount { get; set; }
        [Display(Name = "Badge")]
        public List<string>? Badges { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset EditAt { get; set;}
    }
}
