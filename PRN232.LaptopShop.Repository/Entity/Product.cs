using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN232.LaptopShop.Repository.Entity;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int? StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Category Category { get; set; } = null!;
}
