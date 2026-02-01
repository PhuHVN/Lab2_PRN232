using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.LaptopShop.Service.DTO
{
    public class ProductResponse
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public int? StockQuantity { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
