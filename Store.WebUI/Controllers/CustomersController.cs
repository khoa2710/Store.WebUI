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

        public async Task<IActionResult> Index(string? customerName, string? email, string? phone,
                                       DateTimeOffset? createFrom, DateTimeOffset? createTo)
        {
            var query = _customerRepository.Customers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                query = query.Where(c => c.Name.ToLower().Contains(customerName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(c => c.Email.ToLower().Contains(email.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(c => c.Phone.Contains(phone));
            }
            if (createFrom.HasValue)
            {
                query = query.Where(c => c.CreateAt >= createFrom.Value);
            }
            if (createTo.HasValue)
            {
                query = query.Where(c => c.CreateAt <= createTo.Value);
            }

            var items = await query.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone,
                CreateAt = c.CreateAt
            }).ToListAsync();

            var model = new HomeCustomersViewModel
            {
                Customers = items,
                CustomerName = customerName,
                Email = email,
                Phone = phone,
                CreateFrom = createFrom,
                CreateTo = createTo
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
