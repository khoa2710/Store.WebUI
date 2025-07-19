// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;
using Store.WebUI.Models.SubmitModel;
using Store.WebUI.Models.ViewModel.EditByIdViewModel;
using Store.WebUI.Models.ViewModel.GetByIdViewModel;
using Store.WebUI.Models.ViewModel.HomeViewModel;
using Store.WebUI.Repositories;
using Store.WebUI.Services;

namespace Store.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public readonly IUploadService _uploadService;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IUploadService uploadService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _uploadService = uploadService;
        }

        public async Task<IActionResult> Index(string? productName, string? categoryName, decimal? priceFrom,decimal? priceTo )
        {
            var query = _productRepository.Products.Include(x => x.Category).AsNoTracking();

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(x => x.Name.ToLower().Contains(productName.ToLower()));
            }

            if (!string.IsNullOrEmpty(categoryName)) 
            {
                query = query.Where(x => x.Category.Name.ToLower().Contains(categoryName.ToLower()));
            }
            if (priceFrom.HasValue)
            {
                query = query.Where(p => p.Price >= priceFrom.Value);
            }

            if (priceTo.HasValue)
            {
                query = query.Where(p => p.Price <= priceTo.Value);
            }
            var items = await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Badges = p.Badges,
                CategoryId = p.CategoryId,
                CategoryName = p.Category!.Name,
                Description = p.Description,
                DiscountAmount = p.DiscountAmount,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).ToListAsync();

            var model = new HomeProductsViewModel
            {
                Products = items,
                ProductName = productName,
                CategoryName = categoryName,
                PriceFrom = priceFrom,
                PriceTo = priceTo
                

            };
            return View(model);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var query = _productRepository.Products
                                          .Include(p => p.Category)
                                          .AsNoTracking()
                                          .Where(p => p.Id == id);

            var item = await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Badges = p.Badges,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Description = p.Description,
                DiscountAmount = p.DiscountAmount,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).FirstOrDefaultAsync();

            if (item == null)
                return NotFound();

            var model = new ProductsGetByIdViewModel
            {
                Product = item
            };
            return View(model);
        }

        public async Task<IActionResult> AddProduct()
        {
            // init categories for selectbox/dropdownlist
            var query = _categoryRepository.Categories.AsNoTracking();
            var category = await query.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Icon = x.Icon,
                Slug = x.Slug,
            }).ToListAsync();
                    
            category.Insert(0, new CategoryDto()
            {
                Id = 0,
                Name = "Select a category"
            });
            //ViewBag.Categories = category;
            // set data to view/layout
            ViewData["categories"] = category;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductSubmitModel model)
        {
            
            if (ModelState.IsValid)
            {
                if (!_uploadService.ExtensionWhiteList(model.ProductImage) || !_uploadService.SizeRestriction(model.ProductImage))
                {
                    return BadRequest("Invalid file type or size");
                }

                var standardizedName = _uploadService.StandardizeFileName(model.ProductImage.FileName);
                var imageLink = await _uploadService.SaveFileAsync(model.ProductImage);
                if (imageLink.StartsWith("Cannot"))
                {
                    return BadRequest(imageLink);

                }
                var entity = new Product
                {
                    Name = model.Name!,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = imageLink,
                    StockQuantity = model.StockQuantity,
                    CategoryId = model.CategoryId,
                    IsActive = model.IsActive,
                    DiscountAmount = model.DiscountAmount,
                    Badges = model.Badges,
                    CreateAt = DateTimeOffset.UtcNow,
                    EditAt = DateTimeOffset.UtcNow,
                };

                _productRepository.Add(entity);
                await _productRepository.SaveChangeAsync();
                //
                return RedirectToAction(nameof(Index));
            }
            // get category for select in invalid case
            var query = _categoryRepository.Categories.AsNoTracking();
            var category = await query.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Icon = x.Icon,
                Slug = x.Slug,
            }).ToListAsync();
            category.Insert(0, new CategoryDto()
            {
                Id = 0,
                Name = "Select a category"
            });
            ViewData["categories"] = category;
            return View(model);

        }


        public async Task<IActionResult> EditById(int id)
        {
            var categories = _categoryRepository.Categories.AsNoTracking();
            var category = await categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Icon = x.Icon,
                Slug = x.Slug,
            }).ToListAsync();
            category.Insert(0, new CategoryDto()
            {
                Id = 0,
                Name = "Select a category"
            });
            ViewData["categories"] = category;

            // 
            var model = await _productRepository.Products.AsNoTracking().Where(x => x.Id == id).Select(x => new EditProductSubmitModel 
            {
                Id =x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Badges = string.Join(",", x.Badges!),
                CategoryId = x.CategoryId,
                DiscountAmount = x.DiscountAmount,
                IsActive = x.IsActive,          
                Price = x.Price,
                StockQuantity = x.StockQuantity
            }).FirstOrDefaultAsync();
            
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditById(int id, EditProductSubmitModel model) 
        {
            var query = _productRepository.Products.AsNoTracking();
            var item = await query.FirstOrDefaultAsync(p => p.Id == model.Id);
            if (query == null)
            {
                return NotFound();
            }
            string savedLink = model.ImageUrl!;

            if (model.ProductImage != null)
            {
                if (!_uploadService.ExtensionWhiteList(model.ProductImage) || !_uploadService.SizeRestriction(model.ProductImage))
                {
                    ModelState.AddModelError("Image Type","Invalid image type");
                }

                var standardizedName = _uploadService.StandardizeFileName(model.ProductImage.FileName);
                var imageLink = await _uploadService.SaveFileAsync(model.ProductImage);
                if (imageLink.StartsWith("Cannot"))
                {
                    ModelState.AddModelError("Save File", imageLink);
                }
                savedLink = imageLink;
            }

            if (ModelState.IsValid)
            {
                item!.Name = model.Name;
                item.Description = model.Description;
                item.ImageUrl = savedLink;
                item.StockQuantity = model.StockQuantity;
                item.Price = model.Price;
                item.IsActive = model.IsActive;
                item.CategoryId = model.CategoryId;
                item.DiscountAmount = model.DiscountAmount;
                item.Badges = model.Badges!.Split(',').ToList();
                item.EditAt = DateTimeOffset.UtcNow;
                _productRepository.Edit(item);
                await _productRepository.SaveChangeAsync();
                
                
            }
            var categories = _categoryRepository.Categories.AsNoTracking();
            var category = await categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Icon = x.Icon,
                Slug = x.Slug,
            }).ToListAsync();
            category.Insert(0, new CategoryDto()
            {
                Id = 0,
                Name = "Select a category"
            });
            ViewData["categories"] = category;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteById(int id)
        {
            var entity = await _productRepository.Products.Include(x=>x.OrderDetails).FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return NotFound();
            }               
            if (entity!.OrderDetails.Any())
            {
                return BadRequest();
            }
            
            
            _productRepository.Delete(entity);
            await _productRepository.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
