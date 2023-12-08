using Microsoft.Identity.Client;

namespace MedSysProject.Models
{
    public class CProductWarp
    {
        private Product _Product = null;
        public CProductWarp() {
            _Product = new Product();
        }
        public Product Product { get { return _Product; } set { _Product = value; } }
        public int ProductId { get {return this._Product.ProductId; } set {this._Product.ProductId=value; } }

        public string ProductName { get { return this._Product.ProductName; } set { _Product.ProductName = value; } }

        public decimal? UnitPrice { get { return _Product.UnitPrice; } set { _Product.UnitPrice = value; } }

        public string License { get { return _Product.License; } set { _Product.License = value; } }

        public string Ingredient { get { return _Product.Ingredient; } set { _Product.Ingredient = value; } }

        public string Description { get { return _Product.Description; } set { _Product.Description = value; } }

        public int? UnitsInStock { get { return _Product.UnitsInStock; } set { _Product.UnitsInStock = value; } }

        public bool? Discontinued { get { return _Product.Discontinued; } set { _Product.Discontinued = value; } }

        public string FimagePath { get { return _Product.FimagePath; } set { _Product.FimagePath = value; } }
        public string[] Path { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<ProductsClassification> ProductsClassifications { get; set; } = new List<ProductsClassification>();

        // 確保 SelectedCategories 不為 null
        public List<int> SelectedCategories { get; set; } = new List<int>();
    }
}
