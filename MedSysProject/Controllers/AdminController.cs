using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        public IActionResult EmpManager()
        {
            string keyword = "";
            IEnumerable<Employee> datas = null;

            if (string.IsNullOrEmpty(keyword))
            {
                datas = from t in _db.Employees
                        select t;
            }

            else
                datas = _db.Employees.Where(p => p.EmployeeName.Contains(keyword) ||
                p.EmployeePhoneNum.Contains(keyword) ||
                p.EmployeeEmail.Contains(keyword));
            return View(datas);
        }

        public IActionResult EmpCreate()
        {
            return View();
        }



            public IActionResult EmpClass()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Product(CKeywordViewModel vm)
        {
            var datas = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = datas.Where(p => p.ProductName.Contains(vm.txtKeyword));
            }

            var defaultImagePath = "/img-product/default-image.jpg"; // 設定預設圖片路徑

            var viewModel = datas.Select(product => new CProductsWrap
            {
                Product = product,
                ImagePath = product.FimagePath != null
                    ? Path.Combine("/img-product", product.FimagePath)
                    : defaultImagePath
            }).ToList();


            return View(viewModel);
        }

        public IActionResult Order()
        {
            //string keyword = "";
            IEnumerable<Order> datas = null;

            //if (string.IsNullOrEmpty(keyword))
            datas = from t in _db.Orders
                        select t;
            //else
            //    datas = db.Orders.Where(p => p.OrderDate.Contains(keyword));
            return View(datas);
        }

        public IActionResult Data()
        {
            return View();
        }

        public IActionResult Report()
        {
            var data = from s in _db.ReportDetails
                       select s;
            return View(data);

        }

        public IActionResult qureyReportDetailAll()
        {
            var data = from s in _db.ReportDetails
                       select s;
            return View(data);

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






        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile formFile)
        {
            // 檢查並創建目錄
            string imagePath = Path.Combine(_host.WebRootPath, "img-product");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            if (formFile != null && formFile.Length > 0)
            {
                // 生成唯一的檔案名稱
                string photoName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                // 構建完整的檔案路徑
                string filePath = Path.Combine(imagePath, photoName);

                // 寫入檔案
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }

                // 更新資料庫中的路徑
                product.FimagePath = photoName;
            }

            // 儲存到資料庫
            _db.Products.Add(product);
            _db.SaveChanges();

            return RedirectToAction("Product");
        }


        public IActionResult Edit(int? id)
        {
            Product x = _db.Products.FirstOrDefault(p => p.ProductId == id);

            if (x == null)
                return RedirectToAction("Product");

            return View(x);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CProductsWrap productId, IFormFile formFile)
        {

            Product pDb = _db.Products.FirstOrDefault(p => p.ProductId == productId.WrappedProductId);
            if (pDb != null)
            {
                if (formFile != null)
                {
                    // 生成唯一的檔案名稱
                    string photoName = Guid.NewGuid().ToString() + ".jpg";

                    // 將圖片存放在 wwwroot/img-product 資料夾中
                    string filePath = Path.Combine(_host.WebRootPath, "img-product", photoName);

                    // 將圖片路徑存入資料庫
                    pDb.FimagePath = "/img-product/" + photoName;

                    // 寫入圖片檔案
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }

                pDb.ProductName = productId.WrappedProductName;
                pDb.UnitsInStock = productId.WrappedUnitsInStock;
                pDb.License = productId.WrappedLicense;
                pDb.UnitPrice = productId.WrappedUnitPrice;
                pDb.Ingredient = productId.WrappedIngredient;
                pDb.Description = productId.WrappedDescription;
                pDb.Discontinued = productId.WrappedDiscontinued;

                _db.Products.Update(pDb);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // 處理異常，或輸出到日誌
                    Console.WriteLine(ex.Message);
                }
            }

            return RedirectToAction("Product");
        }

        public IActionResult Delete(int? id)
        {

            Product x = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (x != null)
            {
                _db.Products.Remove(x);
                _db.SaveChanges();
            }
            return RedirectToAction("Product");
        }



    }
}
