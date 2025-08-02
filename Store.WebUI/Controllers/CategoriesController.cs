using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Store.WebUI.Data;
using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;
using Store.WebUI.Models.SubmitModel;
using Store.WebUI.Models.ViewModel;
using Store.WebUI.Models.ViewModel.GetByIdViewModel;
using Store.WebUI.Models.ViewModel.HomeViewModel;
using Store.WebUI.Repositories;
using System.ComponentModel;

namespace Store.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index(string? categoryName,
                                       string? slug,
                                       DateTimeOffset? createFrom,
                                       DateTimeOffset? createTo,
                                       int page = 1, 
                                       int pageSize = 10) 
        {
            int maxPages = 5;
            if (page <= 0) 
            { 
                page = 1; 
            }
            if (pageSize <= 0) 
            { 
                pageSize = 10; 
            }

            var query = _categoryRepository.Categories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                query = query.Where(c => c.Name.ToLower().Contains(categoryName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                query = query.Where(c => c.Slug.ToLower().Contains(slug.ToLower()));
            }
            if (createFrom.HasValue)
            {
                query = query.Where(c => c.CreateAt >= createFrom.Value);
            }
            if (createTo.HasValue)
            {
                query = query.Where(c => c.CreateAt <= createTo.Value);
            }

            int totalRecords = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            //first page 
            var items = await query
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    Slug = c.Slug,
                    CreateAt = c.CreateAt,
                    Icon = c.Icon,
                })
                .ToListAsync();

            var pageNumbers = BuildPageRange(page, totalPages, maxPages);

            var model = new HomeCategoriesViewModel
            {
                Categories = items,
                CategoryName = categoryName,
                Slug = slug,
                CreateFrom = createFrom,
                CreateTo = createTo,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                PageNumbers = pageNumbers
            };
            return View(model);
        }
        // build range cho navigation bar 1,2,3,4,...
        private List<int> BuildPageRange(int current, int total, int max)
        {
            // range mà bé hơn 5 thì show hết tất cả
            if (total <= max) 
            { 
                return Enumerable.Range(1, total).ToList(); 
            }
            //implement cho cái nếu bấm mũi tên tiếp theo thì nó sẽ hiện trang tiếp theo và ngược lại 

            int half = max / 2;
            int start = current - half;
            int end = current + half;

            if (start < 1) 
            { 
                end += 1 - start; start = 1; 
            }
            if (end > total) 
            { 
                start -= end - total; end = total; 
            }

            return Enumerable.Range(start, max).ToList();
        }



        public async Task<IActionResult> GetById(int id)
        {
                
            var query = _categoryRepository.Categories.AsNoTracking().Where(x=> x.Id==id);
            var items = await query.Select(c => new CategoryDto 
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Slug = c.Slug,
                Icon = c.Icon,
            }).FirstOrDefaultAsync();
            if (items == null) 
            {
                return NotFound();
            }
            var model = new CategoriesGetByIdViewModel {
                Category = items,
            };
            return View(model);
        }
        public async Task<IActionResult> AddCategory() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategorySubmitModel model) 
        {
            if (ModelState.IsValid) 
            {
                var entity = new Category {
                    Name = model.Name,
                    Description = model.Description,
                    Icon = model.Icon,
                    Slug = model.Slug,
                    ImageUrl= model.ImageUrl,
                    CreateAt = DateTimeOffset.UtcNow,
                    EditAt = DateTimeOffset.UtcNow,
                };

                _categoryRepository.Add(entity);
                await _categoryRepository.SaveChangeAsync();
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }

        public async Task<IActionResult> EditById(int id) 
        {
            var model = await _categoryRepository.Categories.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new EditCategorySubmitModel
            {
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Icon = x.Icon,
                Slug = x.Slug,
                EditAt= DateTimeOffset.UtcNow
            }).FirstOrDefaultAsync();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditById(int id, EditCategorySubmitModel model) 
        {
            var query = await _categoryRepository.Categories.AsNoTracking().FirstOrDefaultAsync(c=> c.Id == id);
            if (query == null) 
            {
                return NotFound();    
            }
            query.EditAt = DateTimeOffset.UtcNow;
            query.Name = model.Name;    
            query.Description = model.Description;
            query.ImageUrl = model.ImageUrl;
            query.Icon = model.Icon;
            query.Slug = model.Slug;
            _categoryRepository.Edit(query);
            await _categoryRepository.SaveChangeAsync();
            return RedirectToAction(nameof(Index)); 
             
        }

        [HttpPost]
        public async Task<IActionResult> DeleteById(int id)
        {
            var entity = await _categoryRepository.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return NotFound();
            }
            
            _categoryRepository.Delete(entity);
            await _categoryRepository.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
