using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IProductReceiptDetailService
    {
        Task<IEnumerable<ProductReceiptDetailDto>> GetItems();
        Task<IEnumerable<ProductReceiptDetailDto>> GetItemsByVoucherId(int voucherid);
        Task<ProductReceiptDetailDto> GetItem(int id);
        Task<ProductReceiptDetailDto> AddItem(ProductReceiptDetailDto ProductReceiptDetailDto);
        Task<IEnumerable<ProductReceiptDetailDto>> AddItems(IEnumerable<ProductReceiptDetailDto> ProductReceiptDetailDtos);
        Task<ProductReceiptDetailDto> UpdateItem(ProductReceiptDetailDto ProductReceiptDetailDto);
        Task<ProductReceiptDetailDto> UpdateItems(List<ProductReceiptDetailDto> ProductReceiptDetailDtos);
        Task DeleteItem(int id);
    }
}
