using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;

namespace Store.WebUI.Models.Dto
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public string? ProductName { get; set; }


    }
}
