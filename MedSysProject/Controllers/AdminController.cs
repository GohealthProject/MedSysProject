using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm)
        {
            Employee emp = new Employee();

            // 假設 EmployeeClass 具有一個名為 Employees 的屬性，它是 IQueryable<Employee> 的類型

            var user = from i in emp.EmployeeEmail
                       select i;

            if (user != null)
            {
                if (vm.txtPassWord == user)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
