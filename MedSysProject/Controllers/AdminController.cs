using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Linq;
using System.IO;
using NuGet.Protocol;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return View();

            return RedirectToAction("Login");
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

        public IActionResult MemberManager(CKeywordViewModel? vm, int page = 1)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            IEnumerable<Member> datas = null;

            if (string.IsNullOrEmpty(vm.txtKeyword))
            {

                int pgsize = 5;
                int total = _db.Members.Count();
                int maxpage = (total % pgsize == 0 ? total / pgsize : total / pgsize + 1);
                if (page < 1) page = 1;
                if (page > maxpage) page = maxpage;
                datas = _db.Members.Skip((page - 1) * pgsize).Take(pgsize);
                ViewBag.page = page;
                ViewBag.maxpage = maxpage;
                ViewBag.total = total;
                ViewBag.pgsize = pgsize;


                //datas = from t in _db.Employees.Include(p=>p.EmployeeClass)
                //datas = from t in _db.Employees.Include(p => p.EmployeeClass)
                //        select t;

            }

            else
            {
                datas = _db.Members.Where(p => p.MemberName.Contains(vm.txtKeyword) ||
                p.MemberPhone.Contains(vm.txtKeyword) ||
                p.MemberEmail.Contains(vm.txtKeyword));

                ViewBag.key = vm.txtKeyword;
            }

            return View(datas);
        }

        public IActionResult EmpManager(CKeywordViewModel? vm, int page = 1)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            IEnumerable<Employee> datas = null;

            if (string.IsNullOrEmpty(vm.txtKeyword))
            {

                int pgsize = 5;
                int total = _db.Employees.Count();
                int maxpage = (total % pgsize == 0 ? total / pgsize : total / pgsize + 1);
                if (page < 1) page = 1;
                if (page > maxpage) page = maxpage;
                datas = _db.Employees.Include(p => p.EmployeeClass).Skip((page - 1) * pgsize).Take(pgsize);
                ViewBag.page = page;
                ViewBag.maxpage = maxpage;
                ViewBag.total = total;
                ViewBag.pgsize = pgsize;


                //datas = from t in _db.Employees.Include(p=>p.EmployeeClass)
                //datas = from t in _db.Employees.Include(p => p.EmployeeClass)
                //        select t;

            }

            else
            {
                datas = _db.Employees.Where(p => p.EmployeeName.Contains(vm.txtKeyword) ||
                p.EmployeePhoneNum.Contains(vm.txtKeyword) ||
                p.EmployeeEmail.Contains(vm.txtKeyword));

                ViewBag.key = vm.txtKeyword;
            }

            return View(datas);
        }

        public IActionResult EmpJSON()
        {
            IEnumerable<Employee> datas = null;
            datas = from t in _db.Employees.Include(p => p.EmployeeClass)
                    select t;
            return Json(datas);
        }

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


        public IActionResult BlogList(int? id, CKeywordViewModel vm)
        {//
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            IEnumerable<CBlogModel> blogs = null;
            if (id == null)
            {
                if (string.IsNullOrEmpty(vm.txtKeyword))
                {
                    blogs = from blog in _db.Blogs
                            select new CBlogModel
                            {
                                BlogID = blog.BlogId,
                                Title = blog.Title,
                                ArticleClassID = blog.ArticleClassId,
                                Category = blog.ArticleClass.BlogCategory1,
                                Views = blog.Views,
                                CreatedAt = blog.CreatedAt,
                                Content = blog.Content,
                                BlogImage = blog.BlogImage,
                                AuthorID = blog.EmployeeId,
                                AuthorName = blog.Employee.EmployeeName
                            };
                }
                else
                {
                    blogs = from blog in _db.Blogs
                            where blog.Title.Contains(vm.txtKeyword) ||
                                  blog.Employee.EmployeeName.Contains(vm.txtKeyword) ||
                                  blog.ArticleClass.BlogCategory1.Contains(vm.txtKeyword)
                            select new CBlogModel
                            {
                                BlogID = blog.BlogId,
                                Title = blog.Title,
                                ArticleClassID = blog.ArticleClassId,
                                Category = blog.ArticleClass.BlogCategory1,
                                Views = blog.Views,
                                CreatedAt = blog.CreatedAt,
                                Content = blog.Content,
                                BlogImage = blog.BlogImage,
                                AuthorID = blog.EmployeeId,
                                AuthorName = blog.Employee.EmployeeName
                            };
                }

            }
            else
            {
                if (string.IsNullOrEmpty(vm.txtKeyword))
                {
                    blogs = from blog in _db.Blogs
                            where blog.EmployeeId == id
                            select new CBlogModel
                            {
                                BlogID = blog.BlogId,
                                Title = blog.Title,
                                ArticleClassID = blog.ArticleClassId,
                                Category = blog.ArticleClass.BlogCategory1,
                                Views = blog.Views,
                                CreatedAt = blog.CreatedAt,
                                Content = blog.Content,
                                BlogImage = blog.BlogImage,
                                AuthorID = blog.EmployeeId,
                                AuthorName = blog.Employee.EmployeeName
                            };
                }
                else
                {
                    blogs = from blog in _db.Blogs
                            where (blog.EmployeeId == id) && (blog.Title.Contains(vm.txtKeyword) ||
                                  blog.Employee.EmployeeName.Contains(vm.txtKeyword) ||
                                  blog.ArticleClass.BlogCategory1.Contains(vm.txtKeyword))
                            select new CBlogModel
                            {
                                BlogID = blog.BlogId,
                                Title = blog.Title,
                                ArticleClassID = blog.ArticleClassId,
                                Category = blog.ArticleClass.BlogCategory1,
                                Views = blog.Views,
                                CreatedAt = blog.CreatedAt,
                                Content = blog.Content,
                                BlogImage = blog.BlogImage,
                                AuthorID = blog.EmployeeId,
                                AuthorName = blog.Employee.EmployeeName
                            };
                }

            }
            return View(blogs);
        }

        public IActionResult PlanAdd()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            var q = from p in _db.Plans
                    select p;


            return View(q);
        }




        public IActionResult Order()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            //string keyword = "";
            IEnumerable<Order> datas = null;

            //if (string.IsNullOrEmpty(keyword))
            datas = from t in _db.Orders.Include(m => m.Member).Include(s => s.State)
                    select t;
            //else
            //    datas = db.Orders.Where(p => p.OrderDate.Contains(keyword));
            return View(datas);
        }

        public IActionResult Data()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            return View();
        }

        public IActionResult Report(CKeywordViewModel vm)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            IEnumerable<ReportDetail> datas = null;
            //List<CReportWrap> datas2 = null;
            //datas2 = new CReportWrap().Report();
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from s in _db.ReportDetails.Include(p => p.Item)
                        orderby s.ReportId
                        select s;

            else
                datas = _db.ReportDetails.Where(p =>
                p.ReportId.Equals(Convert.ToInt32(vm.txtKeyword)));
            return View(datas);


        }

        public IActionResult ReportDelete(int? id)
        {
            ReportDetail rd = _db.ReportDetails.First (p => p.ReportDetailId == id);
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



        public async Task<IActionResult> Product(CKeywordViewModel vm)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return RedirectToAction("Login");

            // 初始化 ViewBag.Categories
            ViewBag.Categories = await _db.ProductsClassifications
                .Include(pc => pc.Categories)
                .Select(pc => pc.Categories)
                .Distinct()
                .ToListAsync();

            var datas = _db.Products.AsQueryable();

            var keyword = vm.txtKeyword?.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                datas = datas.Where(p =>
                    p.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Ingredient.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.License.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));

                ViewBag.key = keyword;
            }

            var defaultImagePath = "/img-product/default-image.jpg";

            if (vm.txtMinPrice.HasValue)
            {
                datas = datas.Where(p => p.UnitPrice.HasValue && p.UnitPrice.Value >= vm.txtMinPrice.Value);
            }

            if (vm.txtMaxPrice.HasValue)
            {
                datas = datas.Where(p => p.UnitPrice.HasValue && p.UnitPrice.Value <= vm.txtMaxPrice.Value);
            }

            var viewModel = await datas.Select(product => new CProductsWrap
            {
                Product = product,
                ImagePath = product.FimagePath != null
                    ? Path.Combine("/img-product", product.FimagePath)
                    : defaultImagePath
            }).ToListAsync();

            return View(viewModel);
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
                    // 記錄或處理錯誤...

                    // 輸出所有錯誤信息到控制台
                    foreach (var key in ModelState.Keys)
                    {
                        var errors = ModelState[key].Errors;
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                        }
                    }

                    return PartialView("_EditProductModal"); // 或者重新顯示表單
                }

                // 新增此部分以處理 AJAX 請求，獲取產品詳細資訊
                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var product = _db.Products
                        .Where(p => p.ProductId == model.WrappedProductId)
                        .Select(p => new
                        {
                            p.ProductId,
                            p.ProductName,
                            p.UnitPrice,
                            p.License,
                            p.Ingredient,
                            p.Description,
                            p.UnitsInStock,
                            p.Discontinued,
                            p.FimagePath
                        })
                        .FirstOrDefault();

                    return Json(new { success = true, product });
                }

                // 非 AJAX 請求，執行原有的編輯邏輯
                Product pDb = _db.Products.FirstOrDefault(p => p.ProductId == model.WrappedProductId);
               

                if (pDb != null)
                {
                    // 如果原有的圖片路徑不為空，則將其轉為集合
                    var existingImagePaths = !string.IsNullOrEmpty(pDb.FimagePath)
                        ? pDb.FimagePath.Split(',').ToList()
                        : new List<string>();

                    // 合併新上傳的圖片路徑和現有的圖片路徑
                    var combinedImagePaths = new List<string>();
                    if (model.FimagePaths != null && model.FimagePaths.Any())
                    {
                        combinedImagePaths.AddRange(model.FimagePaths);
                    }
                    if (model.FormFiles != null && model.FormFiles.Count > 0)
                    {
                        foreach (var formFile in model.FormFiles)
                        {
                            // 生成唯一的檔案名稱
                            string photoName = Guid.NewGuid().ToString() + ".jpg";

                            // 將圖片存放在 wwwroot/img-product 資料夾中
                            string filePath = Path.Combine(_host.WebRootPath, "img-product", photoName);

                            // 寫入圖片檔案
                            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(fileStream);
                            }

                            // 將圖片路徑保存到集合中
                            combinedImagePaths.Add("/img-product/" + photoName);
                        }
                    }

                    // 將新的圖片路徑和現有的圖片路徑合併，用逗號隔開
                    pDb.FimagePath = string.Join(",", existingImagePaths.Concat(combinedImagePaths));

                    // 處理其他欄位
                    pDb.ProductName = model.WrappedProductName;
                    pDb.UnitsInStock = model.WrappedUnitsInStock;
                    pDb.License = model.WrappedLicense;
                    pDb.UnitPrice = model.WrappedUnitPrice;
                    pDb.Ingredient = model.WrappedIngredient;
                    pDb.Description = model.WrappedDescription;
                    pDb.Discontinued = model.WrappedDiscontinued;

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
                        _db.SaveChanges();
                        return RedirectToAction("Product");
                    }
                    catch (Exception ex)
                    {
                        // 處理異常，例如記錄日誌
                        return Json(new { success = false, message = ex.Message });
                    }
                }

                // 在這裡加上一個 return 陳述式
                return RedirectToAction("Product");
            }
            catch (Exception ex)
            {
                // 處理異常，例如記錄日誌
                return Json(new { success = false, message = ex.Message });
            }
        }















        public IActionResult Delete(int? id)
        {
            Product x = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (x != null)
            {
                // 刪除相關聯的 ProductsClassification 記錄
                var relatedClassifications = _db.ProductsClassifications
                    .Where(pc => pc.ProductId == id)
                    .ToList();

                _db.ProductsClassifications.RemoveRange(relatedClassifications);
                _db.Products.Remove(x);
                _db.SaveChanges();
            }
            return RedirectToAction("Product");
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
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from s in _db.ReportDetails
                        orderby s.ReportId
                        select s;

            else
                datas = _db.ReportDetails.Where(p =>
                p.ReportId.Equals(Convert.ToInt32(vm.txtKeyword)));
            return View(datas);

        }

        public IActionResult test1(CKeywordViewModel vm)
        {
            IEnumerable<ReportDetail> datas = null;
            //List<CReportWrap> datas2 = null;
            //datas2 = new CReportWrap().Report();
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from s in _db.ReportDetails
                        orderby s.ReportId
                        select s;

            else
                datas = _db.ReportDetails.Where(p =>
                p.ReportId.Equals(Convert.ToInt32(vm.txtKeyword)));
            return Json(datas);

        }

    }
}
