using Store.WebUI.Entity;

namespace Store.WebUI.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        void Add(Product Product);
        void Edit(Product Product);
        void Delete(Product Product);
        Task SaveChangeAsync();
    }
}
