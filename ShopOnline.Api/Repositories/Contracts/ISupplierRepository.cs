using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Contracts
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetItems();
              Task<Supplier> GetItem(int id);
    }
}
