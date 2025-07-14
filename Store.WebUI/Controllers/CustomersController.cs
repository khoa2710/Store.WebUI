using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;
using Store.WebUI.Models.SubmitModel;
using Store.WebUI.Models.ViewModel.GetByIdViewModel;
using Store.WebUI.Models.ViewModel.HomeViewModel;
using Store.WebUI.Repositories;
namespace Store.WebUI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            var query = _customerRepository.Customers.AsNoTracking();
            var items = await query.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone
            }).ToListAsync();

            var model = new HomeCustomersViewModel
            {
                Customers = items 
            };
            return View(model);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var query = _customerRepository.Customers
                                          .AsNoTracking()
                                          .Where(c => c.Id == id);

            var item = await query.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone
            }).FirstOrDefaultAsync();

            if (item == null)
                return NotFound();

            var model = new CustomersGetByIdViewModel
            {
                Customer = item
            };
            return View(model);
        }

        public IActionResult AddCustomer()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerSubmitModel model)
        {
            if (!ModelState.IsValid) 
            { 
                return View(model); 
            }

            var entity = new Customer
            {
                Name = model.Name,
                Email = model.Email,
                Address = model.Address,
                Phone = model.Phone,
                CreateAt = DateTimeOffset.UtcNow,
                EditAt = DateTimeOffset.UtcNow
            };

            _customerRepository.Add(entity);
            await _customerRepository.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditById(int id)
        {
            var entity = await _customerRepository.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null) 
            { 
                return NotFound(); 
            }

            var model = new EditCustomerSubmitModel
            {
                Name = entity.Name,
                Email = entity.Email,
                Address = entity.Address,
                Phone = entity.Phone
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditById(int id, EditCustomerSubmitModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = await _customerRepository.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Email = model.Email;
            entity.Address = model.Address;
            entity.Phone = model.Phone;
            entity.EditAt = DateTimeOffset.UtcNow;
            _customerRepository.Edit(entity);
            await _customerRepository.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteById(int id)
        {
            var entity = await _customerRepository.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null)
            {
                return NotFound();
            }
            _customerRepository.Delete(entity);
            await _customerRepository.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
