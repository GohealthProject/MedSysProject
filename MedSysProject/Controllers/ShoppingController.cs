using Azure;
using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NuGet.Packaging.Signing;
using System.Collections.Generic;
using System.Text.Json;

namespace MedSysProject.Controllers
{
    public class ShoppingController : Controller
    {
        MedSysContext? _db = null;
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
                item.Path = item.FimagePath.Split(',');
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
            var product = _db.Products.Include(n => n.ProductsClassifications).ThenInclude(n => n.Categories).FirstOrDefault(n => n.ProductId == id);
            
            if (product == null)
                return RedirectToAction("index");
            if((bool)product.Discontinued&& product != null)
            {
                CProductWarp cp =  new CProductWarp();
                cp.Product = product;
                cp.Path = product.FimagePath.Split(",");
                return View(cp);
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
            List<CCartItem>? cart = null;
            var q = _db.Products.Find(Int32.Parse(data["id"]));
            string? json = "";
            string? count = "";
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
                string? json = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
                List<CCartItem>? cart = JsonSerializer.Deserialize<List<CCartItem>>(json);
                return View(cart);
            }
        }
        public IActionResult KeySearch(string Key)
        {
            if (Key == null)
                return RedirectToAction("index");
            List<CProductWarp> list =new List<CProductWarp>();

            var q = _db.Products.Where(n=>n.ProductName.Contains(Key));
            foreach(var item in q)
            {
                CProductWarp cp = new CProductWarp();
                cp.Product = item;
                list.Add(cp);
            }
            ViewBag.KeySearch = Key;
            return View(list);
        }
        public IActionResult OrderList()
        {
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
            List<COrderWarp>list = new List<COrderWarp>();

            //分頁
            //int pageSize = 5;
            //int total = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Count();
            //int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
            //if (page < 1)
            //    page = 1;
            //if (page > totalPage)
            //    page = totalPage;
            //ViewBag.Page = page;
            //ViewBag.NextPage = page + 1;
            //ViewBag.Total = total;
            //ViewBag.TotalPage = totalPage;
            //ViewBag.PageSize = pageSize;


            //var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == m.MemberId).OrderByDescending(n=>n.OrderDate).Skip((page - 1) * pageSize).Take(pageSize);

            var q = _db.Orders.Include(n => n.Pay).Include(n => n.State).Include(n => n.Ship).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == m.MemberId).OrderByDescending(n => n.OrderDate);
            foreach (var item in q)
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
            if(key == "keyword")
            {
                List<COrderWarp> list = new List<COrderWarp>();
                string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
                MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
                var word = Request.Form["Keyword"];
                if (word == "")
                {
                    ////分頁
                    //int pageSize = 5;
                    //int total = _db.Orders.Where(n => n.MemberId == m.member.MemberId ).Count();
                    //int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                    //if (page < 1)
                    //    page = 1;
                    //if (page > totalPage)
                    //    page = totalPage;
                    //ViewBag.Page = page;
                    //ViewBag.Total = total;
                    //ViewBag.TotalPage = totalPage;
                    //ViewBag.PageSize = pageSize;

                    //var midFindOrder = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(n => n.OrderDate).Skip((page - 1) * pageSize).Take(pageSize);

                    var midFindOrder = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(n => n.OrderDate);

                    foreach (var item in midFindOrder)
                    {
                        COrderWarp o = new COrderWarp();
                        o.order = item;
                        list.Add(o);
                    }
                    return View(list);
                }
                else
                {
                    string? keyword = Request.Form["keyword"];
                    List<int> pids = new List<int>();
                    List<int> oids = new List<int>();

                    pids = _db.Products.Where(n => n.ProductName.Contains(keyword)).Select(n => n.ProductId).ToList();
                    oids = _db.Members.Where(n => n.MemberId == m.MemberId).Include(n => n.Orders).ThenInclude(n => n.OrderDetails).SelectMany(n => n.Orders.Where(n => n.OrderDetails.Any(n => pids.Contains((int)n.ProductId))).Select(n => n.OrderId)).ToList();



                    //分頁
                    //int pageSize = 5;
                    //int total = _db.Orders.Where(n => oids.Contains(n.OrderId)).Count();
                    //int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                    //if (page < 1)
                    //    page = 1;
                    //if (page > totalPage)
                    //    page = totalPage;
                    //ViewBag.Page = page;
                    //ViewBag.Total = total;
                    //ViewBag.TotalPage = totalPage;
                    //ViewBag.PageSize = pageSize;

                    //var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => oids.Contains(n.OrderId)).OrderByDescending(n => n.OrderDate).Skip((page - 1) * pageSize).Take(pageSize);

                    var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => oids.Contains(n.OrderId));
                    foreach (var o in q)
                    {
                        COrderWarp od = new COrderWarp();
                        od.order = o;
                        list.Add(od);
                    }

                    return View(list);
                }
            }else if(key == "dateKey")
            {
                var form = Request.Form;
                List<COrderWarp> list = new List<COrderWarp>();
                DateTime min = DateTime.Parse(form["dateMin"]);
                DateTime max =DateTime.Parse(form["dateMax"]);
                List<int> oids = new List<int>();
                string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
                MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
                oids = _db.Orders.Where(n => n.MemberId == m.MemberId).Select(n=>n.OrderId).ToList();

                //分頁
                //int pageSize = 5;
                //int total = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Count();
                //int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                //if (page < 1)
                //    page = 1;
                //if (page > totalPage)
                //    page = totalPage;
                //ViewBag.Page = page;
                //ViewBag.Total = total;
                //ViewBag.TotalPage = totalPage;
                //ViewBag.PageSize = pageSize;

                //var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.OrderDate >= min && n.OrderDate <= max && oids.Contains(n.OrderId)).OrderByDescending(n => n.OrderDate).Skip((page - 1) * pageSize).Take(pageSize);
                var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.OrderDate >= min && n.OrderDate <= max && oids.Contains(n.OrderId)).OrderByDescending(n => n.OrderDate).ToList();
                foreach (var o in q)
                {
                    COrderWarp od = new COrderWarp();
                    od.order = o;
                    list.Add(od);
                }
                return View(list);
            }
            else
            {
                return Content("ok");
            }
        }
        
    }
}
