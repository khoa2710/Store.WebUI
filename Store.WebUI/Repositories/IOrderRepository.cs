
using Store.WebUI.Data;
using Store.WebUI.Entity;
namespace Store.WebUI.Repositories
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void Add(Order Order);
        void Edit(Order Order);
        void Delete(Order Order);
        Task SaveChangeAsync();
    }
}
