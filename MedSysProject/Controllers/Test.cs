using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
