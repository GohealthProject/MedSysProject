using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MedSysProject.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Service()
        {

            return View();
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(classDictionary.SK_LOINGED_USER))
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

            Employee emp = (new MedSysContext()).Employees.FirstOrDefault(
                t => t.EmployeeEmail.Equals(vm.txtEmail) && t.EmployeePassWord.Equals(vm.txtPassWord));

            if (emp != null && emp.EmployeePassWord.Equals(vm.txtPassWord))
            {

                ViewBag.User = emp.EmployeeName;
                string json = JsonSerializer.Serialize(emp);
                HttpContext.Session.SetString(classDictionary.SK_LOINGED_USER, json);
                return RedirectToAction("Index");
            }
            return View();
        }
    

    }
}
