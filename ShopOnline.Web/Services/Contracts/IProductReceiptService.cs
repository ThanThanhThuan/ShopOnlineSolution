using ShopOnline.Models.Dtos;
namespace ShopOnline.Web.Services.Contracts
{
    public interface IProductReceiptService
    {
        Task<IEnumerable<ProductReceiptDto>> GetItems();
        Task<ProductReceiptDto> GetItem(int id);
        Task<ProductReceiptDto> AddItem(ProductReceiptDto productReceiptDto);
        Task<ProductReceiptDto> UpdateItem(ProductReceiptDto productReceiptDto);
        Task<IEnumerable<ProductReceiptDetailDto>> GetDetails();
        Task<ProductReceiptDetailDto> GetDetail(int id);
        Task<int> DeleteItems(List<int> ids);
    }
}
