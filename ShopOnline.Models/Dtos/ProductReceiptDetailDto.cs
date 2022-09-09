using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
    public class ProductReceiptDetailDto
    {
        public int Id { get; set; }
        public int ProductReceiptId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public string? SupplierName { get; set; }
        public DateTime? VoucherDate { get; set; }

    }
}
