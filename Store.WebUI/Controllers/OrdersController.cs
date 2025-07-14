// Controllers/OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;
using Store.WebUI.Models.SubmitModel;
using Store.WebUI.Models.ViewModel.GetByIdViewModel;
using Store.WebUI.Models.ViewModel.HomeViewModel;
using Store.WebUI.Repositories;
using System.Runtime.CompilerServices;
namespace Store.WebUI.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var query = _orderRepository.Orders.AsNoTracking();
            var items = await query.Select(o => new OrderDto
            {
                Id = o.Id,
                Name = o.Name,
                OrderDate = DateTimeOffset.UtcNow,  
                OrderAddress = o.OrderAddress,
                BillingAddress = o.BillingAddress,
                IsActive = o.IsActive,
                Note = o.Note
            }).ToListAsync();

            var model = new HomeOrdersViewModel
            {
                Orders = items
            };
            return View(model);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var query = _orderDetailRepository.OrderDetails
                                        .Include(o => o.Product)
                                        .AsNoTracking()
                                        .Where(o => o.OrderId == id);

            var item = await query.Select(o => new OrderDetailDto
            {
                //// -- ORDER DETAIL -
                
                OrderId = o.OrderId,
                ProductId = o.ProductId,
                ProductName = o.Product!.Name,
                Quantity = o.Quantity,
                Price = o.Price
            }).FirstOrDefaultAsync();

            if (item == null) 
            { 
                return NotFound(); 
            }

            var model = new OrdersGetByIdViewModel
            {
                Order = item
            };
            return View(model);
        }

        public async Task<IActionResult> AddOrder()  
        {

            var customerQuery = _customerRepository.Customers.AsNoTracking();
            var customer = await customerQuery.Select(c => new CustomerDto 
            { 
                Id = c.Id, 
                Name = c.Name,
                Address = c.Address, 
                Email = c.Email,
                Phone = c.Phone,
            }).ToListAsync();
            customer.Insert(0, new CustomerDto 
            {
                Id = 0, 
                Name = "Select Customer"
            });
            ViewData["customers"] = customer;
            //product dropdownlist
            var productQuery = _productRepository.Products.AsNoTracking();
            var product = await productQuery.Select(p => new ProductDto 
            { 
                Id = p.Id,
                Name = p.Name,
                Badges = p.Badges,
                CategoryId = p.CategoryId,
                Description = p.Description,
                DiscountAmount = p.DiscountAmount,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
            }).ToListAsync();
            product.Insert(0, new ProductDto
            {
                Id = 0,
                Name = "Select Product"
            });
            ViewData["products"] = product;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderSubmitModel model) 
        {
            if (ModelState.IsValid) 
            {
                var order = new Order
                {
                    Name = model.Name,
                    OrderAddress = model.OrderAddress,
                    OrderDate = DateTimeOffset.UtcNow,
                    BillingAddress = model.BillingAddress,
                    Status = model.Status,
                    IsActive = model.IsActive,
                    Note = model.Note,
                    CustomerId = model.CustomerId,
                    CreateAt = DateTimeOffset.UtcNow,  
                    EditAt = DateTimeOffset.UtcNow,
                };
                _orderRepository.Add(order);
                await _orderRepository.SaveChangeAsync();
                var orderDetail = new OrderDetail
                {   
                    Name = order.Name,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    OrderId = order.Id,
                    ProductId = model.ProductId,
                    CreateAt = DateTimeOffset.UtcNow,
                    EditAt = DateTimeOffset.UtcNow,
                };
                
                _orderDetailRepository.Add(orderDetail);
                await _orderDetailRepository.SaveChangeAsync();
            }
            //customer dropdown list
            var customerQuery = _customerRepository.Customers.AsNoTracking();
            var customer = await customerQuery.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Email = c.Email,
                Phone = c.Phone,
            }).ToListAsync();
            customer.Insert(0, new CustomerDto
            {
                Id = 0,
                Name = "Select Customer"
            });
            ViewData["customers"] = customer;


            //product dropdown list
            var productQuery = _productRepository.Products.AsNoTracking();
            var product = await productQuery.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Badges = p.Badges,
                CategoryId = p.CategoryId,
                Description = p.Description,
                DiscountAmount = p.DiscountAmount,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
            }).ToListAsync();
            product.Insert(0, new ProductDto
            {
                Id = 0,
                Name = "Select Product"
            });
            ViewData["products"] = product;

            return RedirectToAction(nameof(Index));
                
        }



        public async Task<IActionResult> EditById(int id) 
        {
            var customerQuery = _customerRepository.Customers.AsNoTracking();
            var customer = await customerQuery.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Email = c.Email,
                Phone = c.Phone,
            }).ToListAsync();
            customer.Insert(0, new CustomerDto
            {
                Id = 0,
                Name = "Select Customer"
            });
            ViewData["customers"] = customer;
            //product dropdownlist
            var productQuery = _productRepository.Products.AsNoTracking();
            var product = await productQuery.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Badges = p.Badges,
                CategoryId = p.CategoryId,
                Description = p.Description,
                DiscountAmount = p.DiscountAmount,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
            }).ToListAsync();
            product.Insert(0, new ProductDto
            {
                Id = 0,
                Name = "Select Product"
            });
            ViewData["products"] = product;

            var order = await _orderRepository.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

            var detail = await _orderDetailRepository.OrderDetails.AsNoTracking().FirstOrDefaultAsync(d => d.OrderId == id);

            if (order == null || detail == null) {
                return NotFound();
            }
               
            var model = new EditOrderSubmitModel
            {
                Id = order.Id,
                Name = order.Name,
                CustomerId = order.CustomerId,
                OrderAddress = order.OrderAddress,
                BillingAddress = order.BillingAddress,
                Status = order.Status,
                IsActive = order.IsActive,
                Note = order.Note,
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                Price = detail.Price
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditById(int id, EditOrderSubmitModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _orderRepository.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);
                var orderDetail = await _orderDetailRepository.OrderDetails.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == model.Id);
                if (order == null && orderDetail == null)
                {
                    return NotFound();
                }
                order!.EditAt = DateTimeOffset.UtcNow;
                order.Name = model.Name;
                order.CustomerId = model.CustomerId;
                order.OrderAddress = model.OrderAddress;
                order.BillingAddress = model.BillingAddress;
                order.Status = model.Status;
                order.IsActive = model.IsActive;
                order.Note = model.Note;
                orderDetail!.Quantity = model.Quantity;
                orderDetail.Price = model.Price;
                orderDetail.ProductId = model.ProductId;
                orderDetail.EditAt = DateTimeOffset.UtcNow;
                _orderRepository.Edit(order);
                await _orderRepository.SaveChangeAsync();
                _orderDetailRepository.Edit(orderDetail);
                await _orderDetailRepository.SaveChangeAsync();
                return RedirectToAction(nameof(Index));
            }
            var customerQuery = _customerRepository.Customers.AsNoTracking();
            var customer = await customerQuery.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Email = c.Email,
                Phone = c.Phone,
            }).ToListAsync();
            customer.Insert(0, new CustomerDto
            {
                Id = 0,
                Name = "Select Customer"
            });
            ViewData["customers"] = customer;
            //product dropdownlist
            var productQuery = _productRepository.Products.AsNoTracking();
            var product = await productQuery.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Badges = p.Badges,
                CategoryId = p.CategoryId,
                Description = p.Description,
                DiscountAmount = p.DiscountAmount,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
            }).ToListAsync();
            product.Insert(0, new ProductDto
            {
                Id = 0,
                Name = "Select Product"
            });
            ViewData["products"] = product;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteById(int id)
        {
            var entity = await _orderRepository.Orders.Include(x=>x.Customer).FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return NotFound();
            }
            if (entity.OrderDetails.Any())
            {
                return BadRequest();
            }


            _orderRepository.Delete(entity);
            await _orderRepository.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }
    }


}
