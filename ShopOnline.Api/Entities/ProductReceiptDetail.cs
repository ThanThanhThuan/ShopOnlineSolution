namespace ShopOnline.Api.Entities
{
    public class ProductReceiptDetail
    {
        public int Id { get; set; }
        public int ProductReceiptId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
