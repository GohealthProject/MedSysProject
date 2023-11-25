using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class BlogsController : Controller
    {
        private readonly MedSysContext _db;

        public BlogsController(MedSysContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 主畫面
        /// </summary>
        /// <returns></returns>
        public IActionResult IndexOld(int? id) 
        {
            var q = _db.BlogCategories.Where(c => c.BlogClassId == id);

            return View();
        }

        

    }
}
