using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging.Signing;
using System.Collections.Generic;
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
            var q = _db.Products.Include(n => n.ProductsClassifications).ThenInclude(n => n.Categories).FirstOrDefault(n => n.ProductId == id);
            if (q == null)
                return RedirectToAction("index");
            if((bool)q.Discontinued&& q!=null)
            {
                return View(q);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult AddToCart()
        {
            var data = Request.Form;
            List<CCartItem> cart = null;
            var q = _db.Products.Find(Int32.Parse(data["id"]));
            string json = "";
            string count = "";
            if (HttpContext.Session.GetString(CDictionary.SK_ADDTOCART) != null)
            {
                json = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
                count = HttpContext.Session.GetString(CDictionary.SK_CARTLISTCOUNT);
                cart = JsonSerializer.Deserialize<List<CCartItem>>(json);
            }
            else
                cart = new List<CCartItem>();
            
            CCartItem item = new CCartItem();
            item.Product = q;
            item.ProductName = q.ProductName;
            item.UnitPrice = (int)q.UnitPrice;
            item.小計 = Int32.Parse(data["count"]) * (int)q.UnitPrice;
            item.count = Int32.Parse(data["count"]);
            cart.Add(item);
            count = cart.Count().ToString();
            json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CDictionary.SK_ADDTOCART, json);
            HttpContext.Session.SetString(CDictionary.SK_CARTLISTCOUNT, count);
            return Ok();
        }
        public IActionResult getcartList()
        {
            if (HttpContext.Session.GetString(CDictionary.SK_CARTLISTCOUNT)!=null)
            {
                string? count = HttpContext.Session.GetString(CDictionary.SK_CARTLISTCOUNT);
                return Content(count);
            }
            else
            {
                return Content("0");
            }
            
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
        public IActionResult OrderList()
        {
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
            List<COrderWarp>list = new List<COrderWarp>();
            var q = _db.Orders.Include(n => n.Pay).Include(n => n.State).Include(n => n.Ship).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == m.MemberId);
            foreach(var item in q)
            {
                COrderWarp od = new COrderWarp();
                od.order = item;
                list.Add(od);
            }
             
            return View(list);
        }
        [HttpPost]
        public IActionResult OrderList(string key)
        {
            
            List<COrderWarp> list = new List<COrderWarp>();
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
            if (string.IsNullOrEmpty(key))
            {
                var midFindOrder = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Include(n=>n.Pay).Include(n=>n.Ship).Include(n=>n.State);
                foreach(var item in midFindOrder)
                {
                    COrderWarp o = new COrderWarp();
                    o.order = item;
                    list.Add(o);
                }
                return View(list);
            }
            else
            {
                List<int> pids = new List<int>();
                List<int> oids = new List<int>();
                pids = _db.Products.Where(n => n.ProductName.Contains(key)).Select(n=>n.ProductId).ToList();
                oids = _db.Members.Where(n => n.MemberId == m.MemberId).Include(n => n.Orders).ThenInclude(n => n.OrderDetails).SelectMany(n => n.Orders.Where(n => n.OrderDetails.Any(n => pids.Contains((int)n.ProductId))).Select(n=>n.OrderId)).ToList() ;
                var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => oids.Contains(n.OrderId));
                foreach (var o in q)
                {
                    COrderWarp od = new COrderWarp();
                    od.order = o;
                    list.Add(od);
                }

                return View(list);
            }
            
        }
    }
}
