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

            Employee emp = _db.Employees.FirstOrDefault(
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

        public IActionResult Product()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
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
