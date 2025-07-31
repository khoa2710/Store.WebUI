using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.WebUI.Models;
using Store.WebUI.Models.Dto;
using Store.WebUI.Models.ViewModel;
using Store.WebUI.Models.ViewModel.HomeViewModel;
using Store.WebUI.Repositories;

namespace Store.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public HomeController(
            ILogger<HomeController> logger,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            // 1) All categories for the top carousel
            var categories = await _categoryRepository.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl
                })
                .ToListAsync();

            // 2) Newest categories (just-arrived section)
            var newestCategories = await _categoryRepository.Categories
                .AsNoTracking()
                .OrderByDescending(c => c.CreateAt)
                .Take(5)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl
                })
                .ToListAsync();

            // 3) Trending products (Star ≥ 4.0)
            var trendingProducts = await _productRepository.Products
                .AsNoTracking()
                .Where(p => p.Star >= 4m)
                .OrderByDescending(p => p.Star)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Star = p.Star,
                    DiscountAmount = p.DiscountAmount
                })
                .ToListAsync();

            var justArrived = await _productRepository.Products
                .AsNoTracking()
                .OrderBy(p => p.CreateAt)      
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Star = p.Star,
                    DiscountAmount = p.DiscountAmount
                })
                .ToListAsync();

            var model = new HomeIndexViewModel
            {
                Categories = categories,
                NewestCategories = newestCategories,
                TrendingProducts = trendingProducts,
                JustArrivedProducts = justArrived 
            };

            return View(model);
        }

        public IActionResult CreateCategory() => View();
        public IActionResult CreateCustomer() => View();
        public IActionResult Privacy() => View();
        public IActionResult Bai2() => View("Bai2");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
