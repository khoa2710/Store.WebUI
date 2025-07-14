using Store.WebUI.Data;
using Store.WebUI.Entity;
namespace Store.WebUI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> Products => _context.Products;

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }
        public void Edit(Product product)
        {
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void Delete(Product product)
        {
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
        public Task SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
