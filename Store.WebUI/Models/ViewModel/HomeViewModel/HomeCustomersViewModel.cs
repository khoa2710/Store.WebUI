using Store.WebUI.Models.Dto;

namespace Store.WebUI.Models.ViewModel.HomeViewModel
{
    public class HomeCustomersViewModel
    {
        public List<CustomerDto> Customers { get; set; } = new List<CustomerDto>();
    }
}

