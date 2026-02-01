using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.LaptopShop.Service.DTO
{
    public class ProductRequest
    {
        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public int? StockQuantity { get; set; }

        public int? CreatedBy { get; set; }
    }
}
