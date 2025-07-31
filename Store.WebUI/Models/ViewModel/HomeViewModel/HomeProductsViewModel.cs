using Store.WebUI.Models.Dto;
using System;
using System.Collections.Generic;

namespace Store.WebUI.Models.ViewModel.HomeViewModel
{
    public class HomeProductsViewModel
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public List<ProductDto> TrendingProducts { get; set; } = new List<ProductDto>();

        // filters
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public DateTimeOffset? CreateFrom { get; set; }
        public DateTimeOffset? CreateTo { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<int> PageNumbers { get; set; } = new();
    }
}
