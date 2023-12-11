using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class SignalRTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
