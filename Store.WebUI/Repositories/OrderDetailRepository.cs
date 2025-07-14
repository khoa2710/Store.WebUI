

using Store.WebUI.Data;
using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;
using Store.WebUI.Repositories;

namespace WebAPI.Repositories
{
    public class OrderDetailRepository: IOrderDetailRepository
    {
        private readonly AppDbContext _context;

        public OrderDetailRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<OrderDetail> OrderDetails => _context.OrderDetails;


        public void Add(OrderDetail od)
        { 
            _context.OrderDetails.Add(od);
        }
        

        public void Delete(OrderDetail od)
        {
            _context.Entry(od).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
       
        public void Edit(OrderDetail orderDetail)
        {
            _context.Entry(orderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public Task SaveChangeAsync( )
        {
            return _context.SaveChangesAsync();
        }
    }
}
