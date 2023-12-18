using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class TestAdminController : Controller
    {
        //注入
        private readonly MedSysContext  _db;
        public TestAdminController(MedSysContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        //會員管理區塊--------------------------------------------------------------
        public IActionResult Member()
        {
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
        //會員管理區塊--------------------------------------------------------------

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

        public IActionResult Plan()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }

        public IActionResult ProductJSON(int? id)
        {
            if (id != null)
            {
                return Json(_db.Products.Find(id));
            }
            else
            {
                return Json(_db.Products);
            }
        }


        
    }
}
