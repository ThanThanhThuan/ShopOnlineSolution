using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;
namespace ShopOnline.Api.Repositories.Contracts
{
    public interface IProductReceiptDetailRepository
    {
        Task<ProductReceiptDetail> GetItem(int id);
        Task<IEnumerable<ProductReceiptDetail>> GetItems();
        Task<IEnumerable<ProductReceiptDetail>> GetItemsByVoucherID(int voucherid);
        Task<ProductReceiptDetail> AddItem(ProductReceiptDetailDto productReceiptDetailDto);
        Task<IEnumerable<ProductReceiptDetail>> AddItems(IEnumerable<ProductReceiptDetailDto> productReceiptDetailDtos);

        Task<ProductReceiptDetail> UpdateItem(ProductReceiptDetailDto productReceiptDetailDto);
        Task<IEnumerable<ProductReceiptDetail>> UpdateItems(IEnumerable<ProductReceiptDetailDto> productReceiptDetailDtos);
        Task<ProductReceiptDetail> DeleteItem(int id);
    }
}
