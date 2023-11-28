using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class WorkPlaceController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
        public IActionResult malePlanConfirm(string words)
        {
            string word = "this is test";
            words = word;
            return Content(word);

        }

    }
}
