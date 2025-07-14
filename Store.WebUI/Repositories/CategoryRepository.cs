using Store.WebUI.Data;
using Store.WebUI.Entity;
using Store.WebUI.Models.Dto;

namespace Store.WebUI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Category> Categories => _context.Categories;

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(Category category)
        {
            _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public Task SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
