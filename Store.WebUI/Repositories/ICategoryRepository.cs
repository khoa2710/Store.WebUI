using Store.WebUI.Data;
using Store.WebUI.Entity;


namespace Store.WebUI.Repositories
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
        void Add(Category category);
        void Edit(Category category);
        void Delete(Category category);
        Task SaveChangeAsync();
    }
}
