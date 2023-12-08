using MedSysProject.Models;
using System.ComponentModel;

namespace MedSysProject.Models
{
    public class CProductsWrap

    {
        // 都不寫
        //public CProductsWrap()
        //{
        //}
        public CProductsWrap()
        {
            _product = new Product();
        }

        public CProductsWrap(Product product)
        {
            _product = product;
        }
        public CProductsWrap(Product product, IList<ProductsClassification> productsClassifications)
        {
            _product = product;
            SelectedCategories = productsClassifications.Select(e => e.CategoriesId).ToList();
        }

        private Product _product;
        public Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        [DisplayName("產品編號")]
        public int WrappedProductId
        {
            get { return _product.ProductId; }
            set { _product.ProductId = value; }
        }
        [DisplayName("產品名稱")]
        public string? WrappedProductName
        {
            get { return _product.ProductName; }
            set { _product.ProductName = value; }
        }
        [DisplayName("單價")]
        public decimal? WrappedUnitPrice
        {
            get { return _product.UnitPrice; }
            set { _product.UnitPrice = value; }
        }
        [DisplayName("核准證號")]
        public string? WrappedLicense
        {
            get { return _product.License; }
            set { _product.License = value; }
        }
        [DisplayName("有效成分")]
        public string? WrappedIngredient
        {
            get { return _product.Ingredient; }
            set { _product.Ingredient = value; }
        }
        [DisplayName("產品描述")]
        public string? WrappedDescription
        {
            get { return _product.Description; }
            set { _product.Description = value; }
        }

        public string ImagePath { get; set; }

        [DisplayName("圖片")]
        public string FimagePath
        {
            get
            {
                if (FimagePaths.Count > 0)
                {
                    return string.Join(",", FimagePaths);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public IList<string> FimagePaths
        {
            get
            {

                if (string.IsNullOrEmpty(_product.FimagePath))
                {
                    return new List<string>();
                }
                else
                {
                    return _product.FimagePath.Split(',').ToList();
                }
            }
            set { _product.FimagePath = string.Join(",", value); }
        }

        public string WrappedFimagePaths
        {
            get { return string.Join(",", FimagePaths); }
            set { FimagePaths = value.Split(',').ToList(); }
        }

        [DisplayName("產品圖片")]
        public List<IFormFile> FormFiles { get; set; }

        [DisplayName("庫存量")]
        public int? WrappedUnitsInStock
        {
            get { return _product.UnitsInStock; }
            set { _product.UnitsInStock = value; }
        }
        [DisplayName("上架狀態")]
        public bool? WrappedDiscontinued
        {
            get { return _product.Discontinued; }
            set { _product.Discontinued = value; }
        }

        public IFormFile FormFile { get; set; }

        // 新增選擇的分類屬性
        public List<int> SelectedCategories { get; set; } = new List<int>();

        public IList<ProductsCategory> ProductsCategories { get; set; }

        // 確保 SelectedCategories 不為 null

    }
}
