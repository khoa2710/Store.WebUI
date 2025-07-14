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
        public async Task<IActionResult> Index()
        {
            var query = _categoryRepository.Categories.AsNoTracking();
            //paging later
            //list
            var items = await query.Select(c => new CategoryDto 
            { 
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Slug =c.Slug,
                Icon =c.Icon,
            
            }).ToListAsync();

            //binding data
            var model = new HomeCategoriesViewModel
            {
                Categories = items,
                
            };

            return View(model);
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
