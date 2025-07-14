

using Store.WebUI.Entity;

namespace Store.WebUI.Repositories
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> Customers { get; }
        void Add(Customer customer);
        void Edit(Customer customer);
        void Delete(Customer customer);
        Task SaveChangeAsync();
    }
}
