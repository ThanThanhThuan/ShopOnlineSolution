using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ProductReceiptDetailRepository : IProductReceiptDetailRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;
        public ProductReceiptDetailRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        public async Task<ProductReceiptDetail> AddItem(ProductReceiptDetailDto productReceiptDetailDto)
        {
            ProductReceiptDetail item = new ProductReceiptDetail {
                ProductReceiptId = productReceiptDetailDto.ProductReceiptId,
                ProductId = productReceiptDetailDto.ProductId,
                Qty = productReceiptDetailDto.Qty,
                Price = productReceiptDetailDto.Price,
                TotalAmount = productReceiptDetailDto.TotalAmount
            };
            var result = await this.shopOnlineDbContext.ProductReceiptDetails.AddAsync(item);
            await this.shopOnlineDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<ProductReceiptDetail>> AddItems(IEnumerable<ProductReceiptDetailDto> productReceiptDetailDtos)
        {
            List<ProductReceiptDetail> productReceiptDetails = new List<ProductReceiptDetail>();
            foreach (var productReceiptDetailDto in productReceiptDetailDtos)
            {
                ProductReceiptDetail item = new ProductReceiptDetail
                {
                    ProductReceiptId = productReceiptDetailDto.ProductReceiptId,
                    ProductId = productReceiptDetailDto.ProductId,
                    Qty = productReceiptDetailDto.Qty,
                    Price = productReceiptDetailDto.Price,
                    TotalAmount = productReceiptDetailDto.TotalAmount
                };
                var res0 = await this.shopOnlineDbContext.ProductReceiptDetails.AddAsync(item);
                productReceiptDetails.Add(res0.Entity);
            }
        
           
            await this.shopOnlineDbContext.SaveChangesAsync();
            return productReceiptDetails;
        }

        public async Task<ProductReceiptDetail> DeleteItem(int id)
        {
            var item = await this.shopOnlineDbContext.ProductReceiptDetails.FindAsync(id);

            if (item != null)
            {
                this.shopOnlineDbContext.ProductReceiptDetails.Remove(item);
                await this.shopOnlineDbContext.SaveChangesAsync();
            }

            return item;
        }

        public async Task<ProductReceiptDetail> GetItem(int id)
        {
            var productReceiptDetail = await shopOnlineDbContext.ProductReceiptDetails
                            .SingleOrDefaultAsync(p => p.Id == id);
            return productReceiptDetail;
        }

        public async Task<IEnumerable<ProductReceiptDetail>> GetItems()
        {
            var productReceiptDetails = await this.shopOnlineDbContext.ProductReceiptDetails
                                 .ToListAsync();

            return productReceiptDetails;
        }

        public async Task<IEnumerable<ProductReceiptDetail>> GetItemsByVoucherID(int voucherid)
        {
            var productReceiptDetails = await this.shopOnlineDbContext.ProductReceiptDetails
            .Where(x=>x.ProductReceiptId==voucherid).ToListAsync();
            
            return productReceiptDetails;
        }

        public async Task<ProductReceiptDetail> UpdateItem(ProductReceiptDetailDto productReceiptDetailDto)
        {
            var item = await this.shopOnlineDbContext.ProductReceiptDetails.FindAsync(productReceiptDetailDto.Id);

            if (item != null)
            {
                item.ProductReceiptId = productReceiptDetailDto.ProductReceiptId;
                item.ProductId = productReceiptDetailDto.ProductId;
                item.Qty = productReceiptDetailDto.Qty;
                item.Price = productReceiptDetailDto.Price;
                item.TotalAmount = productReceiptDetailDto.TotalAmount;
                await this.shopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }

        public async Task<List<ProductReceiptDetail>> UpdateItems(List<ProductReceiptDetailDto> productReceiptDetailDtos)
        {
            List<ProductReceiptDetail> res = new List<ProductReceiptDetail>();
            foreach (var productReceiptDetailDto in productReceiptDetailDtos)
            {
                var item = await this.shopOnlineDbContext.ProductReceiptDetails.FindAsync(productReceiptDetailDto.Id);

                if (item != null)
                {
                    item.ProductReceiptId = productReceiptDetailDto.ProductReceiptId;
                    item.ProductId = productReceiptDetailDto.ProductId;
                    item.Qty = productReceiptDetailDto.Qty;
                    item.Price = productReceiptDetailDto.Price;
                    item.TotalAmount = productReceiptDetailDto.TotalAmount;
                    await this.shopOnlineDbContext.SaveChangesAsync();
                    res.Add(item);
                }
            }
            return res;
        }

        public Task<IEnumerable<ProductReceiptDetail>> UpdateItems(IEnumerable<ProductReceiptDetailDto> productReceiptDetailDtos)
        {
            throw new NotImplementedException();
        }
    }
}
