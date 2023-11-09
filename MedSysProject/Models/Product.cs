using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal? UnitPrice { get; set; }

    public string? License { get; set; }

    public string? Ingredient { get; set; }

    public string? Description { get; set; }

    public byte[]? Photo { get; set; }

    public int? UnitsInStock { get; set; }

    public bool? Discontinued { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductsClassification> ProductsClassifications { get; set; } = new List<ProductsClassification>();
}
