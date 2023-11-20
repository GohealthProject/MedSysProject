using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MedSysProject.Controllers
{
    public class AdminController : Controller
    {
        private MedSysContext _db;

        
        public AdminController(MedSysContext db)
        {
            _db = db;
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

        public string GetEmployeeClassNameById(int employeeClassId)
        {
            // 在此處使用你的資料存取邏輯，從資料庫中取得對應的名稱
            var employeeClass = _db.EmployeeClasses.FirstOrDefault(e => e.EmployeeClassId == employeeClassId);

            // 確認是否找到相應的 EmployeeClass
            if (employeeClass != null)
            {
                return employeeClass.Class;
            }

            return "";
        }

            public IActionResult EmpClass()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Product()
        {
            string keyword = "";
            IEnumerable<Product> datas = null;

            if (string.IsNullOrEmpty(keyword))
                datas = from t in _db.Products
                        select t;
            else
                datas = _db.Products.Where(p => p.ProductName.Contains(keyword));
            return View(datas);
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
            return View();
        }
    }
}
