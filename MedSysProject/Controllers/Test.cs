using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            IEnumerable<Product> datas = null;
            MedSysContext db =new MedSysContext();
            datas = db.Products.Select(n => n);
            return View(datas);

        }
    }
}
