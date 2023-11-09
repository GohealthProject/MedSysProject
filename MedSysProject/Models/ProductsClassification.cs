using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class ProductsClassification
{
    public int ProductsClassification1 { get; set; }

    public int ProductId { get; set; }

    public int CategoriesId { get; set; }

    public virtual ProductsCategory Categories { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
