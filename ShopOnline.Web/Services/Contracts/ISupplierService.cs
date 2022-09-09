using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetItems();
    }
}
