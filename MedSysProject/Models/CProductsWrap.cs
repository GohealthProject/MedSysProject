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
            FimagePaths = new List<string>(); // 初始化 FimagePaths
        }

        public CProductsWrap(Product product)
        {
            _product = product;
            FimagePaths = !string.IsNullOrEmpty(_product.FimagePath)
                ? _product.FimagePath.Split(',').ToList()
                : new List<string>(); // 初始化 FimagePaths
        }
        public CProductsWrap(Product product, IList<ProductsClassification> productsClassifications)
        {
            _product = product;
            SelectedCategories = productsClassifications.Select(e => e.CategoriesId).ToList();
            FimagePaths = !string.IsNullOrEmpty(_product.FimagePath)
                ? _product.FimagePath.Split(',').ToList()
                : new List<string>(); // 初始化 FimagePaths
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
                if (_product?.FimagePath == null)
                {
                    return new List<string>();
                }

                return _product.FimagePath.Split(',').ToList();
            }
            set { _product.FimagePath = value != null ? string.Join(",", value) : string.Empty; }
        }

        public string WrappedFimagePaths
        {
            get { return FimagePaths != null ? string.Join(",", FimagePaths) : string.Empty; }
            set { FimagePaths = value != null ? value.Split(',').ToList() : new List<string>(); }
        }



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

        [DisplayName("產品圖片")]
        public List<IFormFile> FormFiles { get; set; }

        //public IFormFile FormFile { get; set; }

        // 新增選擇的分類屬性
        public List<int> SelectedCategories { get; set; } = new List<int>();

        public IList<ProductsCategory> ProductsCategories { get; set; }

        // 確保 SelectedCategories 不為 null




        //private ProductsCategory _productscategory;
       
        //[DisplayName("選擇產品分類")]
        //public int? WrappedCategoryId
        //{
        //    get { return _productscategory.CategoriesId; }
        //    set => _productscategory.CategoriesId = (int)value;
        //}


    }
}
