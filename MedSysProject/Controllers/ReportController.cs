using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class ReportController : Controller
    {
        private readonly MedSysContext _context;
        public ReportController(MedSysContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
       
     
    }
}
