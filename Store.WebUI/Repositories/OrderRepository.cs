using Store.WebUI.Data;
using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;
namespace Store.WebUI.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Order> Orders => _context.Orders;
        public void Add(Order Order)
        {
            _context.Orders.Add(Order);
        }
        public void Edit(Order Order)
        {
            _context.Entry(Order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void Delete(Order Order)
        {
            _context.Entry(Order).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
        public Task SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
