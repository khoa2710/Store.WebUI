

using Store.WebUI.Entity;

namespace Store.WebUI.Repositories
{
    public interface  IOrderDetailRepository
    {
        IQueryable<OrderDetail> OrderDetails { get; }
        void Add(OrderDetail orderDetail);
        void Edit(OrderDetail orderDetail);
        void Delete(OrderDetail orderDetail);
        Task SaveChangeAsync();
    }
}
