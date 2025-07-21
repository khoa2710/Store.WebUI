using Store.WebUI.Models.Dto;
using System;
using System.Collections.Generic;

namespace Store.WebUI.Models.ViewModel.HomeViewModel
{
    public class HomeCustomersViewModel
    {
        public List<CustomerDto> Customers { get; set; } = new List<CustomerDto>();

        // filters
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTimeOffset? CreateFrom { get; set; }
        public DateTimeOffset? CreateTo { get; set; }
    }
}
