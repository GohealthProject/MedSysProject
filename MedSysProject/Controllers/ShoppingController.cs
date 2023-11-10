using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class ShoppingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
       
    }
}
