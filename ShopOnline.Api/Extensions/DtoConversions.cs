using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                    select new ProductCategoryDto
                    {
                        Id = productCategory.Id,
                        Name = productCategory.Name,
                        IconCSS = productCategory.IconCSS
                    }).ToList();
        }
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from product in products
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = product.ProductCategory.Id,
                        CategoryName = product.ProductCategory.Name
                    }).ToList();

        }
        public static ProductDto ConvertToDto(this Product product)

        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.ProductCategory.Id,
                CategoryName = product.ProductCategory.Name

            };

        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
                                                            IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Price = product.Price,
                        CartId = cartItem.CartId,
                        Qty = cartItem.Qty,
                        TotalPrice = product.Price * cartItem.Qty
                    }).ToList();
        }
        public static CartItemDto ConvertToDto(this CartItem cartItem,
                                                    Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                CartId = cartItem.CartId,
                Qty = cartItem.Qty,
                TotalPrice = product.Price * cartItem.Qty
            };
        }
        public static IEnumerable<ProductReceiptDto> ConvertToDto(this IEnumerable<ProductReceipt> productReceipts)
        {
            return (from productReceipt in productReceipts
                    select new ProductReceiptDto
                    {
                        Id = productReceipt.Id,
                        SupplierId = productReceipt.SupplierId,
                        VoucherDate = productReceipt.VoucherDate
                    }).ToList();
        }
        public static ProductReceiptDto ConvertToDto(this ProductReceipt productReceipt, IEnumerable<Supplier> suppliers)
        {
            var supplierName = suppliers.FirstOrDefault(x => x.Id == productReceipt.SupplierId).SupplierName;
            return new ProductReceiptDto
            {
                Id = productReceipt.Id,
                SupplierId = productReceipt.SupplierId,
                VoucherDate = productReceipt.VoucherDate,
                SupplierName = supplierName 
                   };
        }
        public static IEnumerable<ProductReceiptDetailDto> ConvertToDto(this IEnumerable< ProductReceiptDetail> productReceiptDetails, IEnumerable< Product> products)
        {
            return (from productReceiptDetail in productReceiptDetails
                    join product in products
                    on productReceiptDetail.ProductId equals product.Id
                    select new ProductReceiptDetailDto
                    {
                        Id = productReceiptDetail.Id,
                        ProductReceiptId = productReceiptDetail.ProductReceiptId,
                        ProductId = productReceiptDetail.ProductId,
                        ProductName = product.Name,
                        Qty = productReceiptDetail.Qty,
                        Price = productReceiptDetail.Price
                    }).ToList();

          
        }
        public static IEnumerable<ProductReceiptDetailDto> ConvertToDto(this IEnumerable<ProductReceiptDetail> productReceiptDetails,
            IEnumerable<Product> products,
            IEnumerable<ProductReceipt> productReceipts,
            IEnumerable<Supplier> suppliers)
        {
            List<ProductReceiptDetailDto> res = new List<ProductReceiptDetailDto>();
            foreach (var prd in productReceiptDetails)
            {
                ProductReceiptDetailDto p = new ProductReceiptDetailDto();
                p.ProductId = prd.ProductId;
                p.Id = prd.Id;
                p.ProductReceiptId = prd.ProductReceiptId;
                      var p1=  products.FirstOrDefault(x=> x.Id == prd.ProductId);
                p.ProductName = p1.Name;
                var pr1 = productReceipts.FirstOrDefault(x => x.Id ==prd. ProductReceiptId);
                var s1 = suppliers.FirstOrDefault(x => x.Id == pr1.SupplierId);
                p.Qty = prd.Qty;
                p.Price = prd.Price;
                p.TotalAmount = prd.TotalAmount;
                p.SupplierName = s1.SupplierName;
                p.VoucherDate = pr1.VoucherDate;
                res.Add(p);
            }
            return res;
            //from users in Repo.T_Benutzer
            //from mappings in Repo.T_Benutzer_Benutzergruppen
            //    .Where(mapping => mapping.BEBG_BE == users.BE_ID).DefaultIfEmpty()
            //from groups in Repo.T_Benutzergruppen
            //    .Where(gruppe => gruppe.ID == mappings.BEBG_BG).DefaultIfEmpty()
            //= left join
            //good
            //return (from productReceiptDetail in productReceiptDetails

            //        select new ProductReceiptDetailDto
            //        {
            //            Id = productReceiptDetail.Id,
            //            ProductReceiptId = productReceiptDetail.ProductReceiptId,
            //            ProductId = productReceiptDetail.ProductId,

            //            Qty = productReceiptDetail.Qty,
            //            Price = productReceiptDetail.Price,
            //            TotalAmount = productReceiptDetail.TotalAmount
            //        }).ToList();
            //-----??? Works On LinqPad!
            return (IEnumerable<ProductReceiptDetailDto>)(from productReceiptDetail in productReceiptDetails
                    from product in products
                    .Where (p=>p.Id == productReceiptDetail.ProductId).DefaultIfEmpty()
                    from productReceipt in productReceipts
                    .Where(r => r.Id == productReceiptDetail.ProductReceiptId).DefaultIfEmpty()
                      from supplier in suppliers
                    .Where(s => s.Id == productReceipt.SupplierId).DefaultIfEmpty()
                      select new ProductReceiptDetailDto
                      {
                        Id = productReceiptDetail.Id,
                        ProductReceiptId = productReceiptDetail.ProductReceiptId,
                        ProductId = productReceiptDetail.ProductId,
                        ProductName = product.Name,
                        Qty = productReceiptDetail.Qty,
                        Price = productReceiptDetail.Price,
                        TotalAmount = productReceiptDetail.TotalAmount,
                        SupplierName = supplier.SupplierName,
                        VoucherDate= productReceipt.VoucherDate
    }).ToList();
        
            //return (from productReceiptDetail in productReceiptDetails
            //        join product in products
            //             on productReceiptDetail.ProductId equals product.Id
            //        join productReceipt in productReceipts
            //        on productReceiptDetail.ProductReceiptId equals productReceipt.Id
            //        join supplier in suppliers
            //        on productReceipt.SupplierId equals supplier.Id
            //        select new ProductReceiptDetailDto
            //        {
            //            Id = productReceiptDetail.Id,
            //            ProductReceiptId = productReceiptDetail.ProductReceiptId,
            //            ProductId = productReceiptDetail.ProductId,
            //            ProductName = product.Name,
            //            Qty = productReceiptDetail.Qty,
            //            Price = productReceiptDetail.Price,
            //            TotalAmount = productReceiptDetail.TotalAmount,
            //            SupplierName = supplier.SupplierName,
            //            VoucherDate= productReceipt.VoucherDate
            //        }).ToList();


        }
        public static ProductReceiptDetailDto ConvertToDto(this ProductReceiptDetail productReceiptDetail,Product product)
        {
            return new ProductReceiptDetailDto
            {
                Id = productReceiptDetail.Id,
                ProductReceiptId = productReceiptDetail.ProductReceiptId,
                ProductId = productReceiptDetail.ProductId,
                ProductName = product.Name,
                Qty = productReceiptDetail.Qty,
                Price = productReceiptDetail.Price,
                TotalAmount = productReceiptDetail.TotalAmount

            };
         }
        public static IEnumerable<SupplierDto> ConvertToDto(this IEnumerable<Supplier> suppliers)
        {
            return (from supplier in suppliers
                   select new SupplierDto
                    {
                       Id = supplier.Id,
                       SupplierName = supplier.SupplierName,
                       Address = supplier.Address,
                   }).ToList();
        }
        public static SupplierDto ConvertToDto(this Supplier supplier)
        {
            return new SupplierDto
            {
                Id = supplier.Id,
                SupplierName = supplier.SupplierName,
                Address = supplier.Address,
              
            };
        }
    }
}
