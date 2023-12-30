using Microsoft.EntityFrameworkCore;

namespace MedSysProject.Models.BBL
{
    public class ProductManager
    {
        private MedSysContext _context;

        public ProductManager(MedSysContext context)
        {
            _context = context;
        }

        public CProductWarp? SingleProduct(int Pid)
        {
            var product = _context.Products.Include(n => n.TrackingLists).Include(n => n.ProductsClassifications).ThenInclude(n => n.Categories).FirstOrDefault(n => n.ProductId == Pid);
            
            if(product == null)
            {
                return null;
            }
            if ((bool)product.Discontinued && product != null)
            {
                CProductWarp? productWarp = new CProductWarp();
                productWarp.Product = product;
                productWarp.Path = product.FimagePath.Split(",");

                return productWarp;
            }
            else
            {
                return null;
            }


        }


        public List<CProductWarp> KeySearch(string key)
        {
            List<CProductWarp> result = new List<CProductWarp>();

            var product = _context.Products.Where(n => n.ProductName.Contains(key)).ToList();

            foreach(Product item in product)
            {
                CProductWarp cp = new CProductWarp();
                cp.Product = item;
                cp.Path = item.FimagePath.Split(",");
                result.Add(cp);
            }
            return result;
        }
    }
}
