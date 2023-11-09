using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class ProductsCategory
{
    public int CategoriesId { get; set; }

    public string? CategoriesName { get; set; }

    public virtual ICollection<ProductsClassification> ProductsClassifications { get; set; } = new List<ProductsClassification>();
}
