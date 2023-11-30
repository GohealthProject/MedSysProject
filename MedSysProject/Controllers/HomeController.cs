using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        {//plan選擇，個人版，男女區別，待更改CONTROLL名稱==>自訂方案(含搜尋項目功能)

            return View(_context.Projects) ;
        }
        public IActionResult planComeparison()
        {////方案比較(設計filter篩選方案)
            return View();
        }
        public IActionResult planComeparisonTotal()
        {////方案比較總攬(總項+PDF產生)
            return View();
        }

        /////====start 這裡是partialview區====

        public IActionResult partialvew1()
        { //放選擇方案用的購物車

            return PartialView(_context.Projects);
        }
        public IActionResult partialviewPlan()
        { //plan區==>測試用

            return PartialView(_context.Projects);
        }
        public IActionResult PlanIntroductionProject()
        { //project區==>放方案介紹

            return View();
        }
        public IActionResult partialviewItem()
        { //items區

            return PartialView();
        }
        ///// ====end這裡是partialview區=====
        public IActionResult testplan()
        { //plan 企業版，待更改CONTROLL名稱
            return View();
        }
     
        public IActionResult Reserve()
        { //預約總覽

            //var datas = (from s in _context.PlanRefs.Include(p=>p.Project).ThenInclude(p=>p.Items).ThenInclude(p=>p.)

            //             select s).Distinct();

            var datas = (from s in _context.Plans.Include(p => p.PlanRefs).ThenInclude(p=>p.Project).ThenInclude(p=>p.Items)

                         select s).Distinct();

            return View(datas);
        }
        public IActionResult Member()
        { //會員專項
            return View();
        }

        public IActionResult report(Member id)
        {
            var m = _context.Reserves.Where(s => s.MemberId == 46);


            var j = (from s in _context.ReportDetails
                     where s.Report.MemberId == 46
                     select s.Report.Reserve.ReserveDate).Distinct();

            return View(j);
        }



        public IQueryable<ReportDetail?> qureyReportDetailAll()
        {

            var data = from s in _context.ReportDetails
                       select s;
            return data;
           
           

        }


        public IActionResult live()
        { //現場檢查狀況
          // 模擬每個檢查項目的等待情形
          // 先用假資料
            var healthCheckStatus = new List<string>
    {
        "high", "medium", "low", "low", "high", "medium", "low", "high", "medium", "low",
        "high", "medium", "low", "high", "medium"
    };

            // 將等待情形傳遞到 View
            ViewBag.HealthCheckStatus = healthCheckStatus;
            return View();

        }

        
         
            
         





    }
    }

        
    
