using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MedSysProject.Controllers
{
    public class ShoppingController : Controller
    {
        MedSysContext _db = null;
        public ShoppingController(MedSysContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<CProductWarp> list = new List<CProductWarp>();
            var q = _db.Products.OrderByDescending(n => n.ProductId).Select(n => n).Take(6);
            foreach(var product in q)
            {
                CProductWarp item = new CProductWarp();
                item.Product = product;
                if(item.ProductName.Length > 9)
                {
                    item.ProductName = item.ProductName.Substring(0, 9);
                }
                list.Add(item);
            }
            return View(list);
        }
        public IActionResult Search(int id)
        {
            var q = _db.ProductsCategories.Where(n => n.CategoriesId == id).Include(n => n.ProductsClassifications).ThenInclude(n => n.Product).SelectMany(n => n.ProductsClassifications.Select(n => n.Product));
            List<CProductWarp> ps = new List<CProductWarp>();
            foreach(var p in q)
            {
                CProductWarp c = new CProductWarp();
                c.Product = p;
                ps.Add(c);
            }
            
            return View(ps);
        }
        public IActionResult selectProduct(int id)
        {
            var q = _db.Products.Find(id);
            return View(q);
        }
       public IActionResult AddToCart(CAddToCartViewModel vm)
        {
            List<CCartItem> cart = null;
            var q = _db.Products.Find(vm.id);
            string json = "";
            if (HttpContext.Session.GetString(CDictionary.SK_ADDTOCART)!=null)
            {
                json = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
                cart =JsonSerializer.Deserialize<List<CCartItem>>(json);
            }
            else
                cart = new List<CCartItem>();

                CCartItem item = new CCartItem();
                item.Product = q;
                item.ProductName = q.ProductName;
                item.UnitPrice = (int)q.UnitPrice;
                item.小計 = vm.count * (int)q.UnitPrice;
                item.count = vm.count;
                cart.Add(item);
                json = JsonSerializer.Serialize(cart);
                HttpContext.Session.SetString(CDictionary.SK_ADDTOCART,json);

            return Content("加入成功");
        }
        public IActionResult CartList()
        {
            if (HttpContext.Session.GetString(CDictionary.SK_ADDTOCART) == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
                List<CCartItem> cart = JsonSerializer.Deserialize<List<CCartItem>>(json);
                return View(cart);
            }
        }
        public IActionResult KeySearch(string Key)
        {
            var q = _db.Products.Where(n=>n.ProductName.Contains(Key));
            return View(q);
        }
    }
}
