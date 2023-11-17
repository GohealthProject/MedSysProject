using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Api
{
    public class AccoutApiController : Controller
    {
        private MedSysContext _db = null;
        public AccoutApiController(MedSysContext db)
        {
            _db= db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult checkName(string inName)
        {
            var q = _db.Members.Where(n => n.MemberName == inName);
            if (q.Count() == 0)
            {
                return Content("無");
            }
            else
            {
                return Content("有");
            }
        }
    }
}
