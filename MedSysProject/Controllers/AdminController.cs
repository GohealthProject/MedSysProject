using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Service()
        {
            
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
