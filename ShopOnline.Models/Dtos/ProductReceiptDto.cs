using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
 
        public class ProductReceiptDto
        {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime VoucherDate { get; set; }
        [Required]
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

    }
    
}
