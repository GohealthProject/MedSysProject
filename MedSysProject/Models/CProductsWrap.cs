using MedSysProject.Models;
using System.ComponentModel;

namespace MedSysProject.Models
{
    public class CProductsWrap

    {
        

        public CProductsWrap()
        {
            _product = new Product();
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
            set { Product.ProductId = value; }
        }
        [DisplayName("產品名稱")]
        public string? WrappedProductName
        {
            get { return _product.ProductName; }
            set { Product.ProductName = value; }
        }
        [DisplayName("單價")]
        public decimal? WrappedUnitPrice
        {
            get { return _product.UnitPrice; }
            set { Product.UnitPrice = value; }
        }
        [DisplayName("核准證號")]
        public string? WrappedLicense
        {
            get { return _product.License; }
            set { Product.License= value; }
        }
        [DisplayName("有效成分")]
        public string? WrappedIngredient
        {
            get { return _product.Ingredient; }
            set { Product.Ingredient = value; }
        }
        [DisplayName("產品描述")]
        public string? WrappedDescription
        {
            get { return _product.Description; }
            set { Product.Description = value; }
        }

        public string ImagePath { get; set; }

        [DisplayName("圖片")]
        public string FimagePath { get; set; }


        [DisplayName("庫存量")]
        public int? WrappedUnitsInStock
        {
            get { return _product.UnitsInStock; }
            set { Product.UnitsInStock = value; }
        }
        [DisplayName("上架狀態")]
        public bool? WrappedDiscontinued
        {
            get { return _product.Discontinued; }
            set { Product.Discontinued = value; }
        }

        public IFormFile FormFile { get; set; }

        // 新增選擇的分類屬性
        public List<int> SelectedCategories { get; set; } = new List<int>();


        // 確保 SelectedCategories 不為 null
      
    }
}
