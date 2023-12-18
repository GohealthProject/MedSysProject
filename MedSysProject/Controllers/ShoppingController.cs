using Azure;
using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NuGet.Packaging.Signing;
using OxyPlot;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

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
            var q = _db.Products.Where(n=>n.Discontinued==true).OrderByDescending(n => n.ProductId).Select(n => n).Take(6);
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
            List<int> catList = new List<int>();
            catList  = _db.ProductsCategories.Select(n=>n.CategoriesId).ToList();
            var qq = _db.ProductsClassifications.Where(n => catList.Contains((int)n.CategoriesId)).GroupBy(n => n.CategoriesId).Select(n => new
            {
                n.Key,
                count = n.Count()
            });
            List<int> cat = new List<int>();
            foreach(var item in qq)
            {
                cat.Add(item.count);
            }
            ViewBag.carCount= cat;
            return View(list);
        }
        public IActionResult CatProduct(int id)
        {
            var q = _db.Products.Include(n=>n.ProductsClassifications).ThenInclude(n=>n.Categories).Where(n=>n.ProductsClassifications.Any(n => n.CategoriesId == id));
            List<CProductWarp> ps = new List<CProductWarp>();
            foreach(var p in q)
            {
                CProductWarp c = new CProductWarp();
                c.Product = p;
                c.Path = p.FimagePath.Split(",");
                ps.Add(c);
            }
            List<int> hot6 = new List<int>();
            var qq = _db.OrderDetails
                .Include(n => n.Product)
                .ThenInclude(n => n.ProductsClassifications)
                .ThenInclude(n => n.Categories)
                .Where(n => n.Product.ProductsClassifications.Any(n => n.CategoriesId == id))
                .GroupBy(n => n.ProductId)
                .Select(n => new { 
                    n.Key,
                    sum = n.Sum(n => n.Quantity) 
                })
                .OrderByDescending(n => n.sum);
            foreach(var item in qq)
            {
                hot6.Add((int)item.Key);
            }
            ViewBag.hot = hot6;
            return View(ps);
        }
        public IActionResult selectProduct(int id)
        {
            
            var product = _db.Products.Include(n=>n.TrackingLists).Include(n => n.ProductsClassifications).ThenInclude(n => n.Categories).FirstOrDefault(n => n.ProductId == id);
            
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
        public IActionResult removeCart(int id)
        {
            string json = "";
            List<CCartItem>? cart = null;
            json = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
            cart = JsonSerializer.Deserialize<List<CCartItem>>(json);
            foreach (var item in cart)
            {
                if(item.Product.ProductId== id)
                {
                    cart.Remove(item);
                    break;
                }
            }
            HttpContext.Session.SetString(CDictionary.SK_ADDTOCART, JsonSerializer.Serialize(cart));
            string count = HttpContext.Session.GetString(CDictionary.SK_CARTLISTCOUNT);
            count = (Int32.Parse(count) - 1).ToString();
            HttpContext.Session.SetString(CDictionary.SK_CARTLISTCOUNT, count);
            return RedirectToAction("CartList");
        }
        public IActionResult changeQta(int id,int nqta)
        {
            string json = "";
            List<CCartItem>? cart = null;
            json = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
            cart = JsonSerializer.Deserialize<List<CCartItem>>(json);

            foreach(var item in cart)
            {
                if(item.Product.ProductId == id)
                {
                    item.count= nqta;
                    item.小計 = item.count * item.UnitPrice;
                    break;
                }
            }
            HttpContext.Session.SetString(CDictionary.SK_ADDTOCART, JsonSerializer.Serialize(cart));
            return RedirectToAction("CartList");
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
        public IActionResult paySussess(IFormCollection id)
        {
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }
            var q = _db.Orders.Where(n => n.MerchantTradeNo == data["MerchantTradeNo"]).FirstOrDefault();
            q.TradeNo= data["TradeNo"];
            q.StateId = 14;
            _db.SaveChanges();
            return RedirectToAction("OrderList");
        }
       
        public IActionResult CartList()
        {
            if(!HttpContext.Session.Keys.Contains(CDictionary.SK_ADDTOCART))
                return RedirectToAction("Index");
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            List<CCartItem> cartList = new List<CCartItem>();
            string? json = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
            cartList = JsonSerializer.Deserialize<List<CCartItem>>(json);
            string mjson = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp m = JsonSerializer.Deserialize<MemberWarp>(mjson);

            int total = 0;
            string proName = "";
            foreach (var pro in cartList)
            {
                proName += pro.ProductName + "#";
                total += pro.UnitPrice * pro.count;
            }
            if (proName.Length == 0)
            {
                return RedirectToAction("Index");
            }
            proName = proName.Substring(0, proName.Length - 1);


            //需填入你的網址
            var website = $"https://localhost:7203/";

            var orderGreen = new Dictionary<string, string>
    {
        //綠界需要的參數
        { "MerchantTradeNo",  orderId},
        { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
        { "TotalAmount",  total.ToString()},
        { "TradeDesc",  "無"},
        { "ItemName", proName},
        { "ExpireDate",  "3"},
        { "CustomField1",  ""},
        { "CustomField2",  ""},
        { "CustomField3",  ""},
        { "CustomField4",  ""},
        { "ReturnURL",  $"{website}Shopping/paymethodinfo"},
        { "OrderResultURL", $"{website}Shopping/paySussess/{orderId}"},
        //{ "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
        //{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
        { "MerchantID",  "2000132"},
        { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
        { "PaymentType",  "aio"},
        { "ChoosePayment",  "ALL"},
        { "EncryptType",  "1"},
    };
            //檢查碼
            orderGreen["CheckMacValue"] = PayMethod.GetCheckMacValue(orderGreen);
            ViewBag.order = orderGreen;

            Order o = new Order();
            o.OrderDate = DateTime.Now;
            o.ShipDate = DateTime.Now.AddDays(2);
            o.DeliveryDate = DateTime.Now.AddDays(3);
            o.MemberId = m.MemberId;
            o.PayId = 1;
            o.ShipId = 1;
            o.StateId = 13;
            o.MerchantTradeNo = orderId;
            _db.Orders.Add(o);

            _db.SaveChanges();

            var q =_db.Orders.Where(n=>n.MerchantTradeNo == orderId).FirstOrDefault();

            foreach (var product in cartList)
            {
                OrderDetail od = new OrderDetail();
                od.OrderId = q.OrderId;
                od.ProductId = product.Product.ProductId;
                od.Quantity = product.count;
                od.UnitPrice = product.UnitPrice;
                _db.OrderDetails.Add(od);
            }
            _db.SaveChanges();

            if (HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN) == null)
            {
                return RedirectToAction("Login", "Accout");
            }
            if (HttpContext.Session.GetString(CDictionary.SK_ADDTOCART) == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string? json2 = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
                List<CCartItem>? carts = JsonSerializer.Deserialize<List<CCartItem>>(json2);
                return View(carts);
            }
        }
        //[HttpPost]
        //public IActionResult CartLIst()
        //{
        //    int count = 0;
        //    string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
        //    MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
        //    var data = Request.Form;
        //    var pid = data["ProductID"];
        //    var qta = data["ProductQta"];
        //    var pay = data["odPay"];
        //    var ship = data["odShip"];
        //    Order o = new Order();
        //    o.MemberId = m.MemberId;
        //    o.OrderDate = System.DateTime.Now;
        //    o.PayId = Int32.Parse(pay);
        //    o.ShipId = Int32.Parse(ship);
        //    o.StateId = 2;
        //    o.ShipDate= System.DateTime.Now.AddDays(2);
        //    o.DeliveryDate = System.DateTime.Now.AddDays(3);
        //    _db.Orders.Add(o);
        //    _db.SaveChanges();
        //    var lastOrder = _db.Orders.OrderByDescending(n=>n.OrderId).FirstOrDefault().OrderId;
        //    foreach (var id in pid)
        //    {
        //        var q = _db.Products.Find(Int32.Parse(id));
        //        q.UnitsInStock -= int.Parse(qta[count]);
        //        OrderDetail od = new OrderDetail();
        //        od.ProductId = Int32.Parse(id);
        //        od.Quantity = int.Parse(qta[count]);
        //        od.OrderId = lastOrder;
        //        od.UnitPrice = q.UnitPrice;
        //        _db.OrderDetails.Add(od);
        //        count++;
        //    }
        //    _db.SaveChanges();


        //    return RedirectToAction("index");
        //}
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
                cp.Path = item.FimagePath.Split(",");
                list.Add(cp);
            }
            ViewBag.KeySearch = Key;
            return View(list);
        }
        public IActionResult OrderList(int page=1)
        {
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
            List<COrderWarp>list = new List<COrderWarp>();

            //分頁
            int pageSize = 5;
            int total = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Count();
            int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
            if (page < 1)
                page = 1;
            if (page > totalPage)
                page = totalPage;
            ViewBag.Page = page;
            ViewBag.NextPage = page + 1;
            ViewBag.Total = total;
            ViewBag.TotalPage = totalPage;
            ViewBag.PageSize = pageSize;



            var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == m.MemberId).OrderByDescending(n => n.OrderId).Skip((page - 1) * pageSize).Take(pageSize);

            //var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == m.MemberId).OrderByDescending(n => n.OrderId).Skip((page - 1) * pageSize).Take(pageSize);

            //var q = _db.Orders.Include(n => n.Pay).Include(n => n.State).Include(n => n.Ship).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == m.MemberId).OrderByDescending(n => n.OrderDate);
            foreach (var item in q)
            {
                COrderWarp od = new COrderWarp();
                od.order = item;
                list.Add(od);
            }

             
            return View(list);
        }

        public IActionResult OrderListJSON(int id)
        {

            List<COrderWarp> list = new List<COrderWarp>();
            var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == id);

            //var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == m.MemberId).OrderByDescending(n => n.OrderId).Skip((page - 1) * pageSize).Take(pageSize);

            //var q = _db.Orders.Include(n => n.Pay).Include(n => n.State).Include(n => n.Ship).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.MemberId == m.MemberId).OrderByDescending(n => n.OrderDate);
            foreach (var item in q)
            {
                COrderWarp od = new COrderWarp();
                od.order = item;
                list.Add(od);
            }

            return Json(list);
        }

        [HttpPost]
        public IActionResult OrderList(string key,int page=1)
        {
            if(key == "keyword")
            {
                List<COrderWarp> list = new List<COrderWarp>();
                string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
                MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
                var word = Request.Form["Keyword"];
                if (word == "")
                {
                    //分頁
                    int pageSize = 5;
                    int total = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Count();
                    int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                    if (page < 1)
                        page = 1;
                    if (page > totalPage)
                        page = totalPage;
                    ViewBag.Page = page;
                    ViewBag.NextPage = page + 1;
                    ViewBag.Total = total;
                    ViewBag.TotalPage = totalPage;
                    ViewBag.PageSize = pageSize;

                    var midFindOrder = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(n => n.OrderDate).Skip((page - 1) * pageSize).Take(pageSize);

                    //var midFindOrder = _db.Orders.Where(n => n.MemberId == m.member.MemberId).Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(n => n.OrderDate);

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
                    int total = _db.Orders.Where(n => oids.Contains(n.OrderId)).Count();
                    //int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                    //if (page < 1)
                    //    page = 1;
                    //if (page > totalPage)
                    //    page = totalPage;
                    //ViewBag.Page = page;
                    //ViewBag.NextPage = page + 1;
                    ViewBag.Total = total;
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
                int total = _db.Orders.Where(n => n.OrderDate >= min && n.OrderDate <= max && oids.Contains(n.OrderId)).Count();
                //int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                //if (page < 1)
                //    page = 1;
                //if (page > totalPage)
                //    page = totalPage;
                //ViewBag.Page = page;
                //ViewBag.NextPage = page + 1;
                ViewBag.Total = total;
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
            else if(key == "monthKey")
            {
                var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n=>n.OrderDate.Month == System.DateTime.Now.Month&&n.OrderDate.Year == System.DateTime.Now.Year);
                List<COrderWarp> list = new List<COrderWarp>();
                foreach (var o in q)
                {
                    COrderWarp od = new COrderWarp();
                    od.order = o;
                    list.Add(od);
                }
                int total = list.Count();
                ViewBag.Total = total;
                return View(list);
            }
            else
            {
                return Content("ok");
            }
        }
        
        public IActionResult ProductTracking(int Pid,string heart)
        {
            var q = _db.Products.Find(Pid); 
            if(!HttpContext.Session.Keys.Contains(CDictionary.SK_MEMBER_LOGIN))
            {
                return Ok("請先登入");
            }
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
            List<CProductWarp>? list;

            if (heart == "Noheart.png")
            {
                TrackingList tl = new TrackingList();
                tl.MemberId = m.MemberId;
                tl.ProductId= Pid; 
                _db.TrackingLists.Add(tl);
                _db.SaveChanges();
                return Ok("加入追蹤清單成功");
            }
            else
            {
                _db.TrackingLists.Remove(_db.TrackingLists.FirstOrDefault(n => n.MemberId == m.MemberId && n.ProductId == Pid));
                _db.SaveChanges();
                return Ok("移除追蹤清單成功");
            }
        }

        public IActionResult payMethod()
        {
            return View();
        }

    }
}
