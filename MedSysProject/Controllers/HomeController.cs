using Google.Apis.Auth;
using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using Org.BouncyCastle.Asn1;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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
       
        public IActionResult planComeparison()
        {////方案比較(設計filter篩選方案)
            //_context.Projects.Load();
            //_context.Plans.Load();
            //調整planname個數
            var projectprice = from p in _context.PlanRefs.Include(p => p.Project).Include(p => p.Plan)
                   .AsEnumerable()
                               //from ppp in _context.Plans
                               group p by p.Plan.PlanName into g
                               //select p;
                               select new
                               {
                                   
                PlanName = g.Key,                
                PlanPrice = g.Sum(p => p.Project.ProjectPrice) };

            var total = from p in projectprice
                        from pp in _context.Plans
                        select new { 
                        pp.PlanName,
                        pp.PlanId,
                        
                           //p.PlanName,
                        p.PlanPrice
                        
                        };
            return View(total.ToList());
            //return View(projectprice.ToList());
            //return View(_context.Plans);
        }
        public IActionResult planComeparisonTotal()
        {////方案比較總計(總項+PDF產生)
            return View();
        }

      public IActionResult PlanIntroductionProject(int? id)
        { //放方案介紹  篩對應project和item(以viewModel解決)
            //dynamic方法================
           
            //var WholePlan = _context.Projects.Include(p => p.Items).Include(p => p.PlanRefs).ThenInclude(p => p.Plan);
           
            
            //var ID = .Where(i => i.PlanId == id);
            var joins = from i in _context.Plans.Where(i => i.PlanId == id).Include(i=>i.PlanRefs.Where(i=>i.PlanId==id))
                            //from bg in _context.Items.Include(b=>b.Project).Where(b=>b.ProjectId==_context.Projects.)
                        from pj in _context.Projects.Include(i => i.PlanRefs.Where(j => j.PlanId == id))
                        from it in _context.Items
                        select new
                        {
                            i.PlanId,
                            i.PlanName,
                            i.PlanDescription,
                            //i.PlanRefs,
                            pj.ProjectId,
                            pj.ProjectName,
                            pj.ProjectPrice,
                            it.ItemId,
                            it.ItemName,
                        };
            //dynamic方法================
            //vm方法
            List<CPlanViewModel>data= new List<CPlanViewModel>();
            var plan = from pl in _context.Plans.Where(p => p.PlanId == id)
                       select pl;
          
            foreach (Plan plans in plan)
            {
                data.Add(new CPlanViewModel() { 
                    PlanName = plans.PlanName,
                    PlanDescription=plans.PlanDescription,
                    PlanId = plans.PlanId,
                    
                });
                //data.Add(new CPlanViewModel() { plan=plans });
            }
            var project=from pj in _context.PlanRefs.Where(p=>p.PlanId==id)
                        select pj;
            foreach (PlanRef projects in project)
            {
                data.Add(new CPlanViewModel()
                {ProjectId= (int)projects.ProjectId,
             
                
                });
                    //data.Add(new CPlanViewModel() { planRef=projects });
                }
        var item=from it in _context.Items.Include(i=>i.Project).ThenInclude(i=>i.PlanRefs.Where(i=>i.PlanId==id))
                 select it;

            foreach (Item items in item)
            {
                data.Add(new CPlanViewModel()
                {  ItemId= (int)items.ItemId,
                ItemName= items.ItemName,
                ProjectId= (int)items.ProjectId,
                ProjectName=items.Project.ProjectName,
                ProjectPrice=items.Project.ProjectPrice
            });
                //data.Add(new CPlanViewModel() { item = items });
            }


            return View(data);
            //return View(joins.ToList());
        }
       public IActionResult xxx()
        {//自訂方案加選與總計(含搜尋項目功能):備用

            return View(_context.Projects);
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
  
        public IActionResult partialviewItem()
        { //items區==>測試用

            return PartialView();
        }
        ///// ====end這裡是partialview區=====
        public IActionResult testplan()
        { //plan 企業版，待更改CONTROLL名稱
            return View();
        }

        [HttpPost]
        public IActionResult Reserve(IFormCollection item)
        { //預約總覽

            //var datas = (from s in _context.PlanRefs.Include(p=>p.Project).ThenInclude(p=>p.Items).ThenInclude(p=>p.)

            //             select s).Distinct();

            //var datas = (from s in _context.Plans.Include(p => p.PlanRefs).ThenInclude(p=>p.Project).ThenInclude(p=>p.Items)

            //             select s).Distinct();

            ViewBag.item = item["item"];
            //ViewBag.test = "test123";
            //ViewBag.item = item;

            return View();
        }
        public IActionResult Member()
        { //會員專項
            return View();
        }

        public IActionResult report(int id)
        {
            ViewData["id"] = 55;
            var m = _context.Reserves.Where(s => s.MemberId == 46);

            _context.Members.Load();
            var j = (from s in _context.ReportDetails.Include(p=>p.Item).Include(p=>p.Report).ThenInclude(p=>p.Reserve)
                     where s.Report.MemberId == 7
                     //select s.Report.Reserve.ReserveDate).Distinct();
                     select s);

            return View(j);
        }

        public IActionResult Customcompare()

        {
            //IEnumerable<Item> datas = null;
            //var datas = from s in (_context.Items.Include(p=>p.Project).ThenInclude(p=>p.PlanRefs).ThenInclude(p=>p.Plan)).AsEnumerable().Distinct()
            //            select s;
            _context.Plans.Load();

            var datass = _context.Projects.Include(n => n.Items).Include(n => n.PlanRefs);

            //List<CProjectWarp> list = new List<CProjectWarp>();
            
            //foreach(var item in datass)
            //{
            //    CProjectWarp warp = new CProjectWarp();
            //    warp.project = item;
            //    list.Add(warp);
            //    foreach(var items in item.PlanRefs)
            //    {
            //        var xx = items.Plan;
            //    }
            //}
         
            return View(datass.ToList());
        }

        public IActionResult Customcompare2()

        {
            //IEnumerable<Item> datas = null;
            var datas = from s in (_context.Items.Include(p => p.Project)).AsEnumerable()
                        select s.Project.ProjectName;

            return Ok(datas);
        }


        public IQueryable<ReportDetail?> qureyReportDetailAll()
        {

            var data = from s in _context.ReportDetails
                       select s;
            return data;

           

        }


        public IActionResult Live()
        {
            // 使用 DbContext 取得檢查項目名稱列表
            var healthCheckItems = _context.Projects.Select(p => p.ProjectName).ToList();

            // 模擬每個檢查項目的等待情形
            // 使用隨機生成的資料
            var random = new Random();
            var healthCheckStatus = new List<string>();

            string[] possibleStatus = { "high", "medium", "low" };

            for (int i = 0; i < healthCheckItems.Count; i++)
            {
                int index = random.Next(possibleStatus.Length);
                healthCheckStatus.Add(possibleStatus[index]);
            }

            // 將檢查項目和等待情形傳遞到 View
            ViewBag.HealthCheckItems = healthCheckItems;
            ViewBag.HealthCheckStatus = healthCheckStatus;

            // 取得每個 ProjectId 對應的 ItemName 列表
            var itemNamesByProjectId = new Dictionary<int, List<string>>();

            // 使用 ToList() 將 _context.Projects 查詢的結果讀取到內存中
            var projects = _context.Projects.ToList();

            foreach (var project in projects)
            {
                var itemNames = _context.Items
                    .Where(item => item.ProjectId == project.ProjectId)
                    .Select(item => item.ItemName)
                    .ToList();

                itemNamesByProjectId.Add(project.ProjectId, itemNames);
            }

            ViewBag.ItemNamesByProjectId = itemNamesByProjectId;

            return View();
        }





        public IActionResult payment()
        {
            //step1 : 網頁導入傳值到前端

            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //需填入你的網址
            var website = $"https://localhost:7203/";
            var order = new Dictionary<string, string>
    {
        //綠界需要的參數

        //必填
        { "MerchantID",  "3002599"},//特店編號
        { "MerchantTradeNo",  orderId},//特店訂單編號
        { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},//特店交易時間
        { "PaymentType",  "aio"},//交易類型(固定aio)
        { "TotalAmount",  "1450"},//交易金額
        { "TradeDesc",  "Test"},//交易描述
        { "ItemName",  "測試商品"},//商品名稱
        { "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},//付款完成通知回傳網址
        { "ChoosePayment",  "ALL"},//選擇預設付款方式
        { "EncryptType",  "1"},//CheckMacValue加密類型

        //選填
        { "ExpireDate",  "3"},//分期
        { "CustomField1",  ""},//自訂名稱欄位1
        { "CustomField2",  ""},
        { "CustomField3",  ""},
        { "CustomField4",  ""},
        { "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},//Client端回傳付款結果網址
        //{ "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
        //{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
        { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},//隱藏付款方式
        
        
        
    };
            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);
            return View(order);
        }
        ///待修
        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            //測試用的 HashKey
            var hashKey = "spPjZn66i0OhqJsQ";
            //測試用的 HashIV
            var HashIV = "hT5OJckN45isQTTs";
            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public IActionResult about()
        { //緣由
            return View();
        }




    }
}



