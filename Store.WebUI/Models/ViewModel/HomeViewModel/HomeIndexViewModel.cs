using Store.WebUI.Models.Dto;

namespace Store.WebUI.Models.ViewModel.HomeViewModel
{
    public class HomeIndexViewModel
    {
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

        // 2) Newly Arrived carousel
        public List<CategoryDto> NewestCategories { get; set; } = new List<CategoryDto>();

        // 3) Trending Products grid
        public List<ProductDto> TrendingProducts { get; set; } = new List<ProductDto>();
        public List<ProductDto> JustArrivedProducts { get; set; } = new List<ProductDto>();
    }
}
