using Store.WebUI.Models.Dto;

namespace Store.WebUI.Models.ViewModel.HomeViewModel
{
    public class HomeOrdersViewModel
    {
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
