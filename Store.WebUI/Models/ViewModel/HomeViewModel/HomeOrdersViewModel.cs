using Store.WebUI.Models.Dto;
using System;
using System.Collections.Generic;

namespace Store.WebUI.Models.ViewModel.HomeViewModel
{
    public class HomeOrdersViewModel
    {
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

        // filters
        public string? OrderName { get; set; }
        public DateTimeOffset? CreateFrom { get; set; }
        public DateTimeOffset? CreateTo { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? ProductName { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
    }
}
