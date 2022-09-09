using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Api.Entities
{
    public class ProductReceipt
    {
        public int Id { get; set; }
        public DateTime VoucherDate { get; set; }
        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

    }
}
