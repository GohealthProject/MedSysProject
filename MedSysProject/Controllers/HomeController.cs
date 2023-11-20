using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedSysProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MedSysContext _context;
       public HomeController(ILogger<HomeController> logger, MedSysContext medSysContext)
        {
            _logger = logger;
            _context = medSysContext;
        }

        public IActionResult Index()
        {//版面待修改調整
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult xxx()
        {//plan選擇，個人版，男女區別，待更改CONTROLL名稱
            return View();
        }
        public IActionResult testplan()
        { //plan 企業版，待更改CONTROLL名稱
            return View();
        }
        public IActionResult live()
        { //現場檢查狀況
            return View();
        }
        public IActionResult Reserve()
        { //預約總攬
            return View();
        }
        public IActionResult Member()
        { //會員專項
            return View();
        }

        public IActionResult report(Member id)
        { 
            var m = from s in _context.HealthReports
                    where s.MemberId== id.MemberId
                    select s.ReportDetails;

            return View(m);
        }

    }
}