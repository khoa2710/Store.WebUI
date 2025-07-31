using Store.WebUI.Models.Dto;
using System;
using System.Collections.Generic;

namespace Store.WebUI.Models.ViewModel.HomeViewModel
{
    public class HomeCategoriesViewModel
    {
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

        public string? CategoryName { get; set; }
        public string? Slug { get; set; }
        public DateTimeOffset? CreateFrom { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<int> PageNumbers { get; set; } = new(); 
        public DateTimeOffset? CreateTo { get; set; }
        public string? Icon { get; set; }
    }
}
