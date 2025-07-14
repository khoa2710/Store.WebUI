using Store.WebUI.Data;
using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;
namespace Store.WebUI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Customer> Customers => _context.Customers;

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }
        public void Edit(Customer customer)
        {
            _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void Delete(Customer customer)
        {
            _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
        public Task SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
