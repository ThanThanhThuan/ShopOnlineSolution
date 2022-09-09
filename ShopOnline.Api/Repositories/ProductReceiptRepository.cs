using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ProductReceiptRepository : IProductReceiptRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ProductReceiptRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }
        public async Task<ProductReceipt> AddItem(ProductReceiptDto productReceiptDto)
        {
            //  ProductReceipt item = new ProductReceipt { Id = productReceiptDto.Id, VoucherDate = productReceiptDto.VoucherDate, SupplierId = productReceiptDto.SupplierId };
            ProductReceipt item = new ProductReceipt { VoucherDate = productReceiptDto.VoucherDate, SupplierId = productReceiptDto.SupplierId };
            var result = await this.shopOnlineDbContext.ProductReceipts.AddAsync(item);
            await this.shopOnlineDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ProductReceipt> DeleteItem(int id)
        {
            var item = await this.shopOnlineDbContext.ProductReceipts.FindAsync(id);

            if (item != null)
            {
                this.shopOnlineDbContext.ProductReceipts.Remove(item);
                await this.shopOnlineDbContext.SaveChangesAsync();
            }

            return item;
        }
        public async Task<int> DeleteItems(List<int> ids)
        {
            string ins = string.Join(",", ids);
            string qry = "delete from ProductReceipt where in Id in ({0}); ";
             qry += "delete from ProductReceiptDetail where in ProductReceiptId in ({0}); ";

            var item = await this.shopOnlineDbContext.Database.ExecuteSqlRawAsync(qry,ins);
            return item;
        }

        public async Task<ProductReceipt> GetItem(int id)
        {
            return await(from productreceipt in this.shopOnlineDbContext.ProductReceipts
                        where productreceipt.Id == id
                         select new ProductReceipt
                         {
                             Id = productreceipt.Id,
                             SupplierId = productreceipt.SupplierId,
                             VoucherDate = productreceipt.VoucherDate

                         }).SingleOrDefaultAsync();
        
        }

        public async Task<IEnumerable<ProductReceipt>> GetItems()
        {
            return await(from productreceipt in this.shopOnlineDbContext.ProductReceipts
                         join pcDetail in this.shopOnlineDbContext.ProductReceiptDetails
                         on productreceipt.Id equals pcDetail.ProductReceiptId
                         select new ProductReceipt
                         {
                             Id = productreceipt.Id,
                             SupplierId = productreceipt.SupplierId,
                             VoucherDate = productreceipt.VoucherDate
                         
                         }).ToListAsync();
        }

        public async Task<ProductReceipt> UpdateItem(int id, ProductReceiptDto productReceiptDto)
        {
            var item = await this.shopOnlineDbContext.ProductReceipts.FindAsync(id);

            if (item != null)
            {
                item.SupplierId = productReceiptDto.SupplierId;
                item.VoucherDate = productReceiptDto.VoucherDate;
                await this.shopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}
