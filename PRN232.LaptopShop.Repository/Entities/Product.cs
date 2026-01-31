using System;
using System.Collections.Generic;

namespace PRN232.LaptopShop.Repository.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int? StockQuantity { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
