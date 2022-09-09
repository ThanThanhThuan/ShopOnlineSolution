using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;
        public SupplierRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        public Task<Supplier> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Supplier>> GetItems()
        {
            var suppliers = await this.shopOnlineDbContext.Suppliers
                                     .ToListAsync();

            return suppliers;
        }
    }
}
