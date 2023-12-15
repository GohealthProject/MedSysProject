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

        public IActionResult Member()
        {
            return View();
        }

        //Member Json
        public IActionResult MemberJson()
        {
            return Json(_db.Members);
        }

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
