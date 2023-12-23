using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Linq;
using System.IO;
using NuGet.Protocol;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.IdentityModel.Tokens;
using Humanizer;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Azure;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Collections.Generic;
using static MedSysProject.Controllers.HomeController;
using System.Data;

namespace MedSysProject.Controllers
{
    public class AdminController : Controller
    {
        private MedSysContext _db;
        private readonly IWebHostEnvironment _host;




        public AdminController(MedSysContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }

        public IActionResult Service()
        {

            return View();
        }


        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            List<int> allcounts = new List<int>();

            //Blogs Count Member Count Order Count Report Count
            var members = _db.Members.Count();
            ViewBag.MemberCount = members;

            var reports = _db.ReportDetails.Count();
            ViewBag.ReportCount = reports;

            var orders = _db.Orders.Count();
            ViewBag.OrderCount = orders;

            var blogs = _db.Blogs.Count();
            ViewBag.BlogCount = blogs;

            var products = _db.Products.Count();
            ViewBag.ProductCount = products;

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm)
        {

            Employee? emp = _db.Employees.FirstOrDefault(
                t => t.EmployeeEmail.Equals(vm.txtEmail) && t.EmployeePassWord.Equals(vm.txtPassWord));

            if (emp != null && emp.EmployeePassWord.Equals(vm.txtPassWord))
            {
                string json = JsonSerializer.Serialize(emp);
                HttpContext.Session.SetString(CDictionary.SK_EMPLOYEE_LOGIN, json);
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        //public IActionResult MemberManager(CKeywordViewModel? vm, int page = 1)
        //{
        //    if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
        //        return RedirectToAction("Login");

        //    IEnumerable<Member> datas = null;

        //    if (string.IsNullOrEmpty(vm.txtKeyword))
        //    {

        //        int pgsize = 5;
        //        int total = _db.Members.Count();
        //        int maxpage = (total % pgsize == 0 ? total / pgsize : total / pgsize + 1);
        //        if (page < 1) page = 1;
        //        if (page > maxpage) page = maxpage;
        //        datas = _db.Members.Skip((page - 1) * pgsize).Take(pgsize);
        //        ViewBag.page = page;
        //        ViewBag.maxpage = maxpage;
        //        ViewBag.total = total;
        //        ViewBag.pgsize = pgsize;


        //        //datas = from t in _db.Employees.Include(p=>p.EmployeeClass)
        //        //datas = from t in _db.Employees.Include(p => p.EmployeeClass)
        //        //        select t;

        //    }

        //    else
        //    {
        //        datas = _db.Members.Where(p => p.MemberName.Contains(vm.txtKeyword) ||
        //        p.MemberPhone.Contains(vm.txtKeyword) ||
        //        p.MemberEmail.Contains(vm.txtKeyword));

        //        ViewBag.key = vm.txtKeyword;
        //    }

        //    return View(datas);
        //}

        //會員管理區塊--------------------------------------------------------------
        public IActionResult Member()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            return View();
        }

        //Member Json
        public IActionResult MemberJson(int? id)
        {
            if (id != null)
            {
                return Json(_db.Members.Find(id));
            }
            else
            {
                return Json(_db.Members);
            }
        }

        public IActionResult MemberDetail(int id)
        {
            IEnumerable<Member> data = null;

            data = _db.Members.Where(p => p.MemberId == id);

            if (data == null)
                return RedirectToAction("Member");

            return PartialView("MemberDetail", data);
        }
        //會員管理區塊--------------------------------------------------------------

        //public IActionResult EmpManager(CKeywordViewModel? vm, int page = 1)
        //{
        //    if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
        //        return RedirectToAction("Login");

        //    IEnumerable<Employee> datas = null;

        //    if (string.IsNullOrEmpty(vm.txtKeyword))
        //    {

        //        int pgsize = 5;
        //        int total = _db.Employees.Count();
        //        int maxpage = (total % pgsize == 0 ? total / pgsize : total / pgsize + 1);
        //        if (page < 1) page = 1;
        //        if (page > maxpage) page = maxpage;
        //        datas = _db.Employees.Include(p => p.EmployeeClass).Skip((page - 1) * pgsize).Take(pgsize);
        //        ViewBag.page = page;
        //        ViewBag.maxpage = maxpage;
        //        ViewBag.total = total;
        //        ViewBag.pgsize = pgsize;


        //        //datas = from t in _db.Employees.Include(p=>p.EmployeeClass)
        //        //datas = from t in _db.Employees.Include(p => p.EmployeeClass)
        //        //        select t;

        //    }

        //    else
        //    {
        //        datas = _db.Employees.Where(p => p.EmployeeName.Contains(vm.txtKeyword) ||
        //        p.EmployeePhoneNum.Contains(vm.txtKeyword) ||
        //        p.EmployeeEmail.Contains(vm.txtKeyword));

        //        ViewBag.key = vm.txtKeyword;
        //    }

        //    return View(datas);
        //}

        //員工管理區塊--------------------------------------------------------------
        public IActionResult Employee()
        {
            return View();
        }

        //Employee Json
        public IActionResult EmployeeJson(int? id)
        {
            if (id != null)
            {
                return Json(_db.Employees.Find(id));
            }
            else
            {
                return Json(_db.Employees);
            }
        }

        public IActionResult EmployeeImage(int? id)
        {
            Employee emp = _db.Employees.Find(id);
            byte[]? img = emp?.EmployeePhoto;

            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }
        //員工管理區塊--------------------------------------------------------------

        public IActionResult EmpCreate()
        {
            return View();
        }



        public IActionResult EmpClass()
        {
            return View();
        }

        public IActionResult BlogIndex(int? id, CKeywordViewModel input)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            IEnumerable<Blog> blogs = null;
            if (id == null)
            {
                if (string.IsNullOrEmpty(input.txtKeyword))
                {
                    blogs = from blog in _db.Blogs.Include(blog => blog.Employee)
                                   .Include(blog => blog.ArticleClass)
                            select blog;
                }
                else
                {
                    blogs = from blog in _db.Blogs.Include(blog => blog.Employee).Include(blog => blog.ArticleClass)
                            where blog.Title.Contains(input.txtKeyword) ||
                                  blog.Employee.EmployeeName.Contains(input.txtKeyword) ||
                                  blog.ArticleClass.BlogCategory1.Contains(input.txtKeyword)
                            select blog;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(input.txtKeyword))
                {
                    blogs = from blog in _db.Blogs.Include(blog => blog.Employee)
                          .Include(blog => blog.ArticleClass)
                            where blog.EmployeeId == id
                            select blog;
                }
                else
                {
                    blogs = from blog in _db.Blogs.Include(blog => blog.Employee)
                                  .Include(blog => blog.ArticleClass)
                            where (blog.EmployeeId == id) && (blog.Title.Contains(input.txtKeyword) ||
                                                          blog.Employee.EmployeeName.Contains(input.txtKeyword) ||
                                                          blog.ArticleClass.BlogCategory1.Contains(input.txtKeyword))
                            select blog;
                }
            }
            return View(blogs);
        }



        public IActionResult PlanAdd(CKeywordViewModel vm)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            IEnumerable<Plan> datas = null;
            var qq = _db.Projects.ToList();
            ViewBag.Project = qq;

            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = _db.Plans;
            }
            else
            {
                datas = _db.Plans.Where(p => p.PlanName.Contains(vm.txtKeyword));
                ViewBag.key = vm.txtKeyword;
            }

            return View(datas);
        }

        public IActionResult Order(int page = 1)
        {
            IEnumerable<Order> datas = null; //宣告一個空的集合

            var dd = _db.Orders.ToList();
            var cc = _db.OrderStates.ToList();
            ViewBag.mindate = dd.IsNullOrEmpty() ? "" : dd.Min(p => p.OrderDate).ToString("yyyy-MM-dd");
            ViewBag.maxdate = dd.IsNullOrEmpty() ? "" : dd.Max(p => p.OrderDate).ToString("yyyy-MM-dd");
            ViewBag.State = cc;

            int pgsize = 10; //每頁顯示幾筆資料
            int total = _db.Orders.Count(); //資料總筆數
            int maxpage = (total % pgsize == 0 ? total / pgsize : total / pgsize + 1); //總頁數
            if (page < 1) page = 1; //如果頁數小於1，就顯示第1頁
            if (page > maxpage) page = maxpage; //如果頁數大於總頁數，就顯示最後一頁
            datas = _db.Orders.Include(m => m.Member).Include(s => s.State).Include(h => h.Ship).Include(p => p.Pay).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(d => d.OrderDate).Skip((page - 1) * pgsize).Take(pgsize); //取得資料
            ViewBag.page = page; //目前頁數
            ViewBag.TotalPage = maxpage; //總頁數
            ViewBag.total = total; //資料總筆數
            ViewBag.pgsize = pgsize; //每頁顯示幾筆資料

            return View(datas);
        }

        [HttpPost]
        public IActionResult Order(CStateViewModel? vmS, CKeywordViewModel? vmK, CDateViewModel? vmD, int page = 1)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            //string keyword = "";
            var cc = _db.OrderStates.ToList();
            ViewBag.State = cc;

            //

            IEnumerable<Order> datas = null;

            if (string.IsNullOrEmpty(vmK.txtKeyword))
            {
                //var word = Request.Form["txtKeyword"];
                var dd = _db.Orders.ToList();
                DateTime mindate = dd.IsNullOrEmpty() ? DateTime.MinValue : dd.Min(p => p.OrderDate);
                DateTime maxdate = dd.IsNullOrEmpty() ? DateTime.MaxValue : dd.Max(p => p.OrderDate);
                ViewBag.mindate = mindate.ToString("yyyy-MM-dd");
                ViewBag.maxdate = maxdate.ToString("yyyy-MM-dd");


                if (vmD.txtMinDate.HasValue && vmD.txtMaxDate.HasValue && vmS.statechk == null)
                {
                    int pgsize = 10; //每頁顯示幾筆資料
                    int total = _db.Orders.Count(); //資料總筆數
                    int maxpage = (total % pgsize == 0 ? total / pgsize : total / pgsize + 1); //總頁數
                    if (page < 1) page = 1; //如果頁數小於1，就顯示第1頁
                    if (page > maxpage) page = maxpage; //如果頁數大於總頁數，就顯示最後一頁

                    datas = _db.Orders
                        .Include(m => m.Member)
                        .Include(s => s.State)
                        .Include(h => h.Ship)
                        .Include(p => p.Pay)
                        .Include(n => n.OrderDetails)
                        .ThenInclude(n => n.Product)
                        .OrderByDescending(d => d.OrderDate)
                        .Skip((page - 1) * pgsize)
                        .Take(pgsize);

                    ViewBag.page = page; //目前頁數
                    ViewBag.TotalPage = maxpage; //總頁數
                    ViewBag.total = total; //資料總筆數
                    ViewBag.pgsize = pgsize; //每頁顯示幾筆資料

                    ViewBag.mindate = vmD.txtMinDate.Value.ToString("yyyy-MM-dd");
                    ViewBag.maxdate = vmD.txtMaxDate.Value.ToString("yyyy-MM-dd");
                    ViewBag.Checked = vmS.statechk;
                }
                else if (vmD.txtMinDate.HasValue && vmD.txtMaxDate.HasValue && vmS.statechk != null)
                {
                    datas = _db.Orders
                        //find p.StateID equal to List all numbers in vmS.statechk and p.OrderDate between vmD.txtMinDate and vmD.txtMaxDate
                        .Where(p => vmS.statechk.Contains((int)p.StateId) && p.OrderDate >= vmD.txtMinDate && p.OrderDate <= vmD.txtMaxDate)
                        .Include(m => m.Member)
                        .Include(s => s.State)
                        .Include(h => h.Ship)
                        .Include(p => p.Pay)
                        .Include(n => n.OrderDetails)
                        .ThenInclude(n => n.Product)
                        .OrderByDescending(d => d.OrderDate);

                    ViewBag.mindate = vmD.txtMinDate.Value.ToString("yyyy-MM-dd");
                    ViewBag.maxdate = vmD.txtMaxDate.Value.ToString("yyyy-MM-dd");
                    ViewBag.Checked = vmS.statechk;
                }
                else
                {
                    int pgsize = 10; //每頁顯示幾筆資料
                    int total = _db.Orders.Count(); //資料總筆數
                    int maxpage = (total % pgsize == 0 ? total / pgsize : total / pgsize + 1); //總頁數
                    if (page < 1) page = 1; //如果頁數小於1，就顯示第1頁
                    if (page > maxpage) page = maxpage; //如果頁數大於總頁數，就顯示最後一頁

                    datas = _db.Orders
                            .Include(m => m.Member)
                            .Include(s => s.State)
                            .Include(h => h.Ship)
                            .Include(p => p.Pay)
                            .Include(n => n.OrderDetails)
                            .ThenInclude(n => n.Product)
                            .OrderByDescending(d => d.OrderDate)
                            .Skip((page - 1) * pgsize)
                            .Take(pgsize);

                    ViewBag.page = page; //目前頁數
                    ViewBag.TotalPage = maxpage; //總頁數
                    ViewBag.total = total; //資料總筆數
                    ViewBag.pgsize = pgsize; //每頁顯示幾筆資料

                    ViewBag.mindate = datas.IsNullOrEmpty() ? "" : datas.Min(p => p.OrderDate).ToString("yyyy-MM-dd");
                    ViewBag.maxdate = datas.IsNullOrEmpty() ? "" : datas.Max(p => p.OrderDate).ToString("yyyy-MM-dd");
                    ViewBag.key = vmK.txtKeyword;
                }
            }
            else
            {
                datas = _db.Orders
                    .Include(m => m.Member)
                    .Include(s => s.State)
                    .Include(h => h.Ship)
                    .Include(p => p.Pay)
                    .Include(n => n.OrderDetails)
                    .ThenInclude(n => n.Product)
                    .Where(p => p.OrderId.ToString().Contains(vmK.txtKeyword) ||
                    p.Member.MemberName.Contains(vmK.txtKeyword) ||
                    p.State.StateName.Contains(vmK.txtKeyword)
                    )
                    .OrderByDescending(d => d.OrderDate);

                ViewBag.mindate = datas.IsNullOrEmpty() ? "" : datas.Min(p => p.OrderDate).ToString("yyyy-MM-dd");
                ViewBag.maxdate = datas.IsNullOrEmpty() ? "" : datas.Max(p => p.OrderDate).ToString("yyyy-MM-dd");
                ViewBag.key = vmK.txtKeyword;
            }

            return View(datas);
        }

        public IActionResult OrderDetail(int id)
        {
            IEnumerable<Order> data = null;

            data = _db.Orders.Where(p => p.OrderId == id)
                    .Include(m => m.Member)
                    .Include(s => s.State)
                    .Include(h => h.Ship)
                    .Include(p => p.Pay)
                    .Include(n => n.OrderDetails)
                    .ThenInclude(n => n.Product)
                    .ToList();



            //List<COrderWarp> data = new List<COrderWarp>();
            //var q = _db.Orders
            //    .Include(n => n.Member)
            //    .Include(n => n.Pay)
            //    .Include(n => n.Ship)
            //    .Include(n => n.State)
            //    .Include(n => n.OrderDetails)
            //    .ThenInclude(n => n.Product)
            //    .Where(n => n.OrderId == id);

            //foreach (var item in q)
            //{
            //    COrderWarp od = new COrderWarp();
            //    od.order = item;
            //    data.Add(od);
            //}

            if (data == null)
                return RedirectToAction("Order");

            return PartialView("OrderDetail", data);
        }

        public IActionResult OrderDetailJSON(int id)
        {
            //IEnumerable<Order> data = null;

            //data = _db.Orders.Where(p => p.OrderId == id)
            //        .Include(m => m.Member)
            //        .Include(s => s.State)
            //        .Include(h => h.Ship)
            //        .Include(p => p.Pay)
            //        .Include(n => n.OrderDetails)
            //        .ThenInclude(n => n.Product);

            List<COrderWarp> data = new List<COrderWarp>();
            var q = _db.Orders
                .Include(n => n.Member)
                .Include(n => n.Pay)
                .Include(n => n.Ship)
                .Include(n => n.State)
                .Include(n => n.OrderDetails)
                .ThenInclude(n => n.Product)
                .Where(n => n.OrderId == id);

            //這個訂單有哪些產品


            foreach (var item in q)
            {
                COrderWarp od = new COrderWarp();
                od.order = item;
                data.Add(od);
            }

            if (data == null)
                return RedirectToAction("Order");

            return Json(data);
        }

        //todo 順哥幫我做
        public IActionResult ODProductsJSON(int id)
        {
            IEnumerable<OrderDetail> data = new List<OrderDetail>();
            List<OrderDetail> dataList = _db.OrderDetails.Where(n => n.OrderId == id).Include(n => n.Product).ToList();

            return Json(dataList);
        }

        public IActionResult Data()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            return View();
        }

        public IActionResult Report(CKeywordViewModel vm, int page = 1)
        {
            //尚未完成 :報告寄出提示功能(次要)
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            IEnumerable<ReportDetail> datas = null;
            //List<CReportWrap> datas2 = null;
            //datas2 = new CReportWrap().Report();
            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                int pagesize = 10;
                int total = _db.ReportDetails.Count();
                int maxpage = (total % pagesize == 0 ? total / pagesize : total / pagesize + 1);
                datas = _db.ReportDetails.Include(p => p.Item).OrderByDescending(p => p.ReportId).Skip((page - 1) * pagesize).Take(pagesize);
                ViewBag.page = page; //目前頁數
                ViewBag.maxpage = maxpage; //總頁數
                ViewBag.total = total; //資料總筆數
                ViewBag.pagesize = pagesize; //每頁顯示幾筆資料
            }

            else
            {
                datas = _db.ReportDetails.Where(p =>
                p.ReportId.Equals(Convert.ToInt32(vm.txtKeyword)));
            }
            return View(datas);


        }

        public IActionResult ReportDelete(int? id)
        {
            ReportDetail rd = _db.ReportDetails.First(p => p.ReportDetailId == id);
            if (rd != null)
            {
                _db.ReportDetails.Remove(rd);
                _db.SaveChanges();
            }
            return RedirectToAction("Report");
        }

        public IActionResult ReportEdit(int? id)
        {

            ReportDetail rd = _db.ReportDetails.FirstOrDefault(p => p.ReportDetailId == id);
            if (rd == null)
                return RedirectToAction("Report");
            return View(rd);
        }

        [HttpPost]
        public IActionResult ReportEdit(CReportWrap pIn)
        {
            ReportDetail rdDb = _db.ReportDetails.FirstOrDefault(p => p.ReportDetailId == pIn.ReportDetailId);
            if (rdDb != null)
            {
                rdDb.ReportDetailId = pIn.ReportDetailId;
                rdDb.ItemId = pIn.ItemId;
                rdDb.Result = pIn.Result;
                _db.SaveChanges();
            }
            return RedirectToAction("Report");
        }



        public async Task<IActionResult> Product(CKeywordViewModel vm, string sortOrder, int? categoryId)
        {
            // 檢查使用者是否已登錄，如果未登錄，則重新導向到登錄頁面
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            // 初始化 ViewBag.Categories
            ViewBag.Categories = await _db.ProductsClassifications
                .Include(pc => pc.Categories)
                .Select(pc => pc.Categories)
                .Distinct()
                .ToListAsync();

            // 建立查詢物件
            var datas = _db.Products.AsQueryable();

            // 獲取搜索關鍵字並去除前後空格
            var keyword = vm.txtKeyword?.Trim();

            // 如果關鍵字不為空，則進行搜索
            if (!string.IsNullOrEmpty(keyword))
            {
                // 將關鍵字轉換為小寫以進行不區分大小寫的比較
                keyword = keyword.ToLower();

                // 在記憶體中執行字串包含操作以進行模糊搜索
                datas = datas.Where(p =>
                    p.ProductName.ToLower().Contains(keyword) ||
                    p.Ingredient.ToLower().Contains(keyword) ||
                    p.License.ToLower().Contains(keyword) ||
                    p.Description.ToLower().Contains(keyword)).AsQueryable();

                // 將原始關鍵字傳遞給視圖以在搜尋框中顯示
                ViewBag.key = vm.txtKeyword;
            }

            // 根據 sortOrder 參數對結果集進行排序
            switch (sortOrder)
            {
                case "price_desc":
                    datas = datas.OrderByDescending(p => p.UnitPrice);
                    break;
                default:   // 價格從低到高
                    datas = datas.OrderBy(p => p.UnitPrice);
                    break;
            }

            if (categoryId.HasValue)
            {
                
                datas = datas.Where(p => p.ProductsClassifications.Any(pc => pc.CategoriesId == categoryId.Value));
            }

            ViewBag.Categories = await _db.ProductsCategories.ToListAsync();
            // 設定預設圖片路徑
            var defaultImagePath = "/img-product/default-image.jpg";

            // 根據價格範圍篩選數據
            if (vm.txtMinPrice.HasValue)
            {
                datas = datas.Where(p => p.UnitPrice.HasValue && p.UnitPrice.Value >= vm.txtMinPrice.Value);
            }

            if (vm.txtMaxPrice.HasValue)
            {
                datas = datas.Where(p => p.UnitPrice.HasValue && p.UnitPrice.Value <= vm.txtMaxPrice.Value);
            }

            // 建立視圖模型並使用ToListAsync()將結果轉換為List
            var viewModel = await datas.Select(product => new CProductsWrap
            {
                Product = product,
                ImagePath = product.FimagePath != null
                    ? Path.Combine("/img-product", product.FimagePath)
                    : defaultImagePath
            }).ToListAsync();

            // 返回視圖
            return View(viewModel);
        }



        public List<ProductsCategory> GetCategories()
        {
            // 使用 LINQ 查詢從數據庫中獲取所有類別
            List<ProductsCategory> categories = _db.ProductsCategories.ToList();

            return categories;
        }


        public ActionResult FilterProducts()
        {
            var products = _db.Products.ToList(); // 假設 _db 是您的資料庫上下文
            var categories = GetCategories(); // 假設 GetCategories 是一個獲取類別清單的方法
            var productsWithCategories = products.Select(product => new CProductsWrap(product, product.ProductsClassifications.ToList())).ToList();

            return View(productsWithCategories);
        }






        [HttpPost]
        public IActionResult ToggleDiscontinued(int productId, bool discontinued)
        {
            // 根據 productId 更新數據庫中的 discontinued 屬性
            var product = _db.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                product.Discontinued = discontinued;
                _db.SaveChanges();
            }

            // 返回 JSON 物件，包含更新後的 discontinued 值
            return Json(new { discontinued = product?.Discontinued });
        }

        public IActionResult GetImage(int id)
        {
            Product product = _db.Products.Find(id);

            if (product != null && !string.IsNullOrEmpty(product.FimagePath))
            {
                string imagePath = Path.Combine(_host.WebRootPath, "img-product", product.FimagePath);

                if (System.IO.File.Exists(imagePath))
                {
                    return PhysicalFile(imagePath, "image/jpeg");
                }
            }

            return NotFound();
        }



        //public IActionResult _CreateProductModal()
        //{
        //    ViewBag.Categories = _db.ProductsCategories?.ToList() ?? new List<ProductsCategory>();
        //    return PartialView("_CreateProductModal");
        //}


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _db.ProductsCategories.ToList();
            var cProductsWrap = new CProductsWrap
            {
                // 在這裡初始化相關的屬性，例如 ProductsCategories 和其他相關數據
                ProductsCategories = _db.ProductsCategories.ToList(),
            };

            return PartialView("_CreateProductModal");
        }

        [HttpPost]
        public IActionResult Create(CProductsWrap productWrap, List<IFormFile> formFiles, int[] CategoriesIds)
        {
            if (productWrap.Product == null)
            {
                // 初始化 productWrap.Product，例如：
                productWrap.Product = new Product();
            }

            // 檢查並創建目錄
            string imagePath = Path.Combine(_host.WebRootPath, "img-product");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            List<string> imagePaths = new List<string>();

            foreach (var formFile in formFiles)
            {
                if (formFile != null && formFile.Length > 0)
                {
                    string photoName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                    string filePath = Path.Combine(imagePath, photoName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }

                    imagePaths.Add(photoName);
                }
            }

            productWrap.FimagePaths = imagePaths;

            // 將產品的圖片檔名存儲到資料庫
            productWrap.Product.FimagePath = string.Join(",", imagePaths);
            _db.SaveChanges();



            // 儲存到資料庫
            _db.Products.Add(productWrap.Product);
            _db.SaveChanges();

            // 將產品與所選分類關聯
            if (CategoriesIds != null)
            {
                foreach (var categoryId in CategoriesIds)
                {
                    var classification = new ProductsClassification
                    {
                        ProductId = productWrap.Product.ProductId,
                        CategoriesId = categoryId
                    };

                    _db.ProductsClassifications.Add(classification);
                }

                _db.SaveChanges();
            }

            return PartialView("_CreateProductModal", productWrap);
        }


        [HttpGet]
        public IActionResult Edit(int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            // 將查詢結果轉換為 List
            List<Product> products = _db.Products.Where(e => e.ProductId == productId).ToList();

            if (products.Count == 0)
            {
                return NotFound();
            }

            // 取第一筆資料
            Product product = products.FirstOrDefault();

            if (product == null)
                return NotFound();

            // 在這裡初始化 ViewBag.Categories
            var categories = _db.ProductsCategories.ToList();
            ViewBag.Categories = categories;

            // 將 Product 對象包裝到 CProductsWrap 中
            CProductsWrap productWrap = new CProductsWrap(product);
            productWrap.SelectedCategories = _db.ProductsClassifications
                .Where(e => e.ProductId == productId)
                .Select(e => e.CategoriesId)
                .ToList();

            return PartialView("_EditProductModal", productWrap);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CProductsWrap model, int wrappedProductId)
        {
            try
            {
                // 移除不需要進行模型驗證的屬性
                ModelState.Remove("FormFiles");
                ModelState.Remove("ImagePath");

                if (!ModelState.IsValid)
                {
                    if (IsAjaxRequest())
                    {
                        // 對 AJAX 請求返回 JSON 錯誤信息
                        return Json(new { success = false, message = "模型驗證失敗" });
                    }
                    // 非 AJAX 請求重定向
                    return RedirectToAction("Product");
                }

                // 查找產品
                Product pDb = _db.Products.FirstOrDefault(p => p.ProductId == wrappedProductId);

                if (pDb == null)
                {
                    if (IsAjaxRequest())
                    {
                        // 對 AJAX 請求返回 JSON 錯誤信息
                        return Json(new { success = false, message = "找不到產品" });
                    }
                    // 非 AJAX 請求重定向
                    return RedirectToAction("Product");
                }

                // 更新產品資料
                pDb.ProductName = model.WrappedProductName;
                pDb.UnitsInStock = model.WrappedUnitsInStock;
                pDb.License = model.WrappedLicense;
                pDb.UnitPrice = model.WrappedUnitPrice;
                pDb.Ingredient = model.WrappedIngredient;
                pDb.Description = model.WrappedDescription;
                pDb.Discontinued = model.WrappedDiscontinued;

                // 處理圖片上傳
                if (model.FormFiles != null && model.FormFiles.Count > 0)
                {
                    // 清空原有的圖片路徑
                    pDb.FimagePath = string.Empty;

                    foreach (var file in model.FormFiles)
                    {
                        var uploadsFolder = Path.Combine(_host.WebRootPath, "img-product"); // 圖片保存的文件夾
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        // 更新數據庫中的圖片路徑，將多個圖片路徑以逗號分隔
                        pDb.FimagePath += string.IsNullOrEmpty(pDb.FimagePath) ? uniqueFileName : $",{uniqueFileName}";
                    }
                }

                // 處理 ProductsClassification 資料表
                if (model.SelectedCategories != null && model.SelectedCategories.Any())
                {
                    // 移除所有現有的 CategoriesId 記錄
                    _db.ProductsClassifications.RemoveRange(_db.ProductsClassifications.Where(pc => pc.ProductId == model.WrappedProductId));

                    // 新增選擇的 CategoriesId 記錄
                    foreach (var categoryId in model.SelectedCategories)
                    {
                        var classification = new ProductsClassification
                        {
                            ProductId = model.WrappedProductId,
                            CategoriesId = categoryId
                        };

                        _db.ProductsClassifications.Add(classification);
                    }
                }

                try
                {
                    // 保存變更
                    _db.SaveChanges();

                    if (IsAjaxRequest())
                    {
                        // 對 AJAX 請求返回 JSON 成功信息
                        return Json(new { success = true, message = "產品資料已更新" });
                    }
                    // 非 AJAX 請求重定向
                    return RedirectToAction("Product");
                }
                catch (Exception ex)
                {
                    if (IsAjaxRequest())
                    {
                        // 對 AJAX 請求返回 JSON 錯誤信息
                        return Json(new { success = false, message = ex.Message });
                    }
                    // 非 AJAX 請求重定向
                    return RedirectToAction("Product");
                }
            }
            catch (Exception ex)
            {
                if (IsAjaxRequest())
                {
                    // 對 AJAX 請求返回 JSON 錯誤信息
                    return Json(new { success = false, message = ex.Message });
                }
                // 非 AJAX 請求重定向
                return RedirectToAction("Product");
            }
        }


        private bool IsAjaxRequest()
        {
            return HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }





        public IActionResult Delete(int? id)
        {
            Product x = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (x != null)
            {
                // 刪除與產品相關的圖片
                if (!string.IsNullOrEmpty(x.FimagePath))
                {
                    string[] images = x.FimagePath.Split(',');
                    foreach (var image in images)
                    {
                        string filePath = Path.Combine(_host.WebRootPath, "img-product", image);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                // 刪除產品和相關聯的 ProductsClassification 記錄
                var relatedClassifications = _db.ProductsClassifications
                    .Where(pc => pc.ProductId == id)
                    .ToList();

                _db.ProductsClassifications.RemoveRange(relatedClassifications);
                _db.Products.Remove(x);
                _db.SaveChanges();
            }
            return RedirectToAction("Product");
        }

        [HttpGet]
        public IActionResult ProductDetail(int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            var product = _db.Products.FirstOrDefault(e => e.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            // 在這裡初始化 ViewBag.Categories
            var categories = _db.ProductsCategories.ToList();
            ViewBag.Categories = categories;

            var productWrap = new CProductsWrap(product);
            productWrap.SelectedCategories = _db.ProductsClassifications
                .Where(e => e.ProductId == productId)
                .Select(e => e.CategoriesId)
                .ToList();

            // 使用一個新的 Partial View 或現有的，只顯示不可編輯的資訊
            return PartialView("_DetailProductModal", productWrap);
        }



        public IActionResult GetImageByte(int? id)
        {
            Employee emp = _db.Employees.Find(id);
            byte[]? img = emp?.EmployeePhoto;

            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }

        public string GetMemberName(int? id)
        {
            Member member = _db.Members.Find(id);

            if (member != null)
            {
                return member.MemberName;
            }

            return "";
        }

        public IActionResult test(CKeywordViewModel vm)
        {
            IEnumerable<ReportDetail> datas = null;
            //List<CReportWrap> datas2 = null;
            //datas2 = new CReportWrap().Report();
            //if (string.IsNullOrEmpty(vm.txtKeyword))
            //    datas = from s in _db.ReportDetails
            //            orderby s.ReportId
            //            select s;

            //else
            //    datas = _db.ReportDetails.Where(p =>
            //    p.ReportId.Equals(Convert.ToInt32(vm.txtKeyword)));
            return View(/*datas*/);

        }

        public IActionResult test1(int id = 0)
        {
            //IEnumerable<ReportDetail> datas = null;
            //List<CReportWrap> datas2 = null;
            //datas2 = new CReportWrap().Report();
            //if (id == 0)
            //    datas = from s in _db.ReportDetails
            //            orderby s.ReportId
            //            select s;

            //else
              var  datas = _db.ReportDetails.Where(p => p.Report.ReportId.Equals(id))
                        .Select(p => new{name = p.Item.ItemName, result =p.Result , ReportId = p.Report.ReportId, ReportDetailId=p.ReportDetailId});
            return Json(datas);
            







            //var pl = _context.Plans.Where(p => p.PlanId == planid)
            //   .SelectMany(p => p.PlanRefs, (plan, project) => new { plan, project }).Where(p => p.project.PlanId == planid)
            //   .SelectMany(p => p.project.Project.Items, (prbg, it) => new { prbg.project.Project, it }).Where(p => p.Project.ProjectId == p.it.ProjectId)

            //   //.SelectMany(p => p.project.Project.Items, (projectid, item) => new { projectid, item }).Where(p => p.item.ProjectId == p.projectid.project.ProjectId)
            //   .Select(t => new
            //   {
            //       planId = t.Project.PlanRefs.First().PlanId,
            //       planName = t.Project.PlanRefs.First().Plan.PlanName,
            //       projectid = t.Project.ProjectId,
            //       ProjectName = (string)t.Project.ProjectName,
            //       ProjectPrice = (double)t.Project.ProjectPrice,
            //       itemId = t.it.ItemId,
            //       ItemName = (string)t.it.ItemName,
            //       ItemPrice = (int)t.it.ItemPrice,
            //   });
            ////------datatable 轉json區--------
            //DataTable dt = new DataTable();
            //dt.Columns.Add(new DataColumn("planId"));
            //dt.Columns.Add(new DataColumn("planName"));

            //dt.Columns.Add(new DataColumn("projectid"));
            //dt.Columns.Add(new DataColumn("ProjectName"));
            //dt.Columns.Add(new DataColumn("ProjectPrice"));
            //dt.Columns.Add(new DataColumn("itemId"));
            //dt.Columns.Add(new DataColumn("ItemName"));
            //dt.Columns.Add(new DataColumn("ItemPrice"));
            //foreach (var t in pl)
            //{
            //    DataRow dr = dt.NewRow();

            //    dr["planId"] = t.planId;
            //    dr["PlanName"] = t.planName;

            //    dr["projectid"] = t.projectid;
            //    dr["ProjectName"] = t.ProjectName;
            //    dr["ProjectPrice"] = t.ProjectPrice;
            //    dr["itemId"] = t.itemId;
            //    dr["ItemName"] = t.ItemName;
            //    dr["ItemPrice"] = t.ItemPrice;
            //    dt.Rows.Add(dr);
            //}
            //DataTableToJsonConverter converter = new DataTableToJsonConverter();
            //string js = converter.ConverterDataTableToJson(dt);

            ////------datatable 轉json區--------



            //string json = System.Text.Json.JsonSerializer.Serialize(pl);


            //return js;
        }



    }
}
