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

        public IActionResult Employee()
        {
            return View();
        }

        public IActionResult Plan()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }

        
    }
}
