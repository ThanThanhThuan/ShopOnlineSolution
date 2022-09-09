using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories.Contracts
{
    public interface IProductReceiptRepository
    {
        Task<ProductReceipt> AddItem(ProductReceiptDto productReceiptDto);
        Task<ProductReceipt> UpdateItem(int id, ProductReceiptDto productReceiptDto);
        Task<ProductReceipt> DeleteItem(int id);
        Task<int> DeleteItems(List<int> ids);
        Task<ProductReceipt> GetItem(int id);
        Task<IEnumerable<ProductReceipt>> GetItems();
    }
}
