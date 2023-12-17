using Google.Apis.Auth;
using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using Org.BouncyCastle.Asn1;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using Org.BouncyCastle.Tls;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using System.Text.Json.Serialization.Metadata;

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
        {////方案比較(設計filter篩選方案)+更換圖片+加中文名稱
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
                                   PlanPrice = g.Sum(p => p.Project.ProjectPrice)
                               };

            var total = from p in projectprice
                        from pp in _context.Plans
                        select new
                        {
                            pp.PlanName,
                            pp.PlanId,

                            //p.PlanName,
                            p.PlanPrice

                        };
            return View(total.ToList());
            //return View(projectprice.ToList());
            //return View(_context.Plans);
        }

        
        [HttpGet]
        public IActionResult planComeparisonTotal(string planlist)
        {////方案比較總計(總項+PDF產生)+篩選比較intersection+資料確認(在post已找出完整資料，可複製貼上)+資料匯出
            //int id = 3;
            if (planlist != null)
            {
 List<int> list = new List<int>();

            foreach(var item2 in planlist.Split(','))
            {
                if(item2!="")
                    list.Add(Int32.Parse(item2));

            }
            //ViewBag.Id = id;
            var plan2 = _context.Plans.Where(n => list.Contains(n.PlanId));
            List<CPlanViewModel> data = new List<CPlanViewModel>();
            List<CPlanViewModel> test = new List<CPlanViewModel>();
            //var plan = from pl in _context.Plans.Where(p => p.PlanId == id)
            //           select pl;

            foreach (Plan plans in plan2)
            {
                test.Add(new CPlanViewModel()
                {
                    //PlanName = plans.PlanName,
                    //PlanDescription = plans.PlanDescription,
                    PlanId = plans.PlanId,

                });

                }
                var project = from pj in _context.PlanRefs.Include(p=>p.Project).Where(n => list.Contains(n.PlanId))
                          select pj;
            foreach (PlanRef projects in project)
            {
                data.Add(new CPlanViewModel()
                {
                    ProjectId = (int)projects.ProjectId,
                    ProjectName = projects.Project.ProjectName,
                    ProjectPrice = projects.Project.ProjectPrice,
                    PlanDescription=projects.Plan.PlanDescription,
                    PlanName=projects.Plan.PlanName,
                    PlanId= projects.PlanId,
                });
              
            }

            var item = from it in _context.Items
                       select it;
            //var item = from it in _context.Items.Include(i => i.Project).ThenInclude(i => i.PlanRefs.Where(n => list.Contains(n.PlanId)))
            //           select it;
            int dp = 4;
            //var itemsss2 = _context.Plans.Where(n => n.PlanId == 4).SelectMany(n => n.PlanRefs.SelectMany(p => p.Project.Items.Select(o => new { o.ItemId, o.ItemName }), l => new { (int)l.ProjectId,(int)l.PlanId})
            //));
            //var itemsss = _context.Plans.Where(n => n.PlanId == 4).SelectMany(n => n.PlanRefs.SelectMany(p => p.Project.Items)).Select(p => new
            //{
            //    PlanDescription = (string)p.Project.PlanRefs.SelectMany(p => p.Plan.PlanDescription),
            //    PlanName = (string)p.Project.PlanRefs.SelectMany(p => p.Plan.PlanName),
            //    ProjectId = (int)p.ProjectId,
            //    ItemId = p.ItemId,
            //}).AsEnumerable().ToList();

            //foreach (var p in itemsss)
            //{
            //    test.Add(new CPlanViewModel()
            //    {
            //        PlanDescription = p.PlanDescription,
            //        PlanName = p.PlanName,
            //        ProjectId = (int)p.ProjectId,
            //        ItemId = p.ItemId,


            //        //PlanDescription = (string)p.Project.PlanRefs.SelectMany(p => p.Plan.PlanDescription),
            //        //PlanName = (string)p.Project.PlanRefs.SelectMany(p => p.Plan.PlanName),
            //        //PlanId = dp,
            //        //ProjectId = (int)p.ProjectId,
            //        //ProjectName = (string)p.Project.ProjectName,
            //        ////ProjectPrice = p.PlanRefs.SelectMany(pp => pp.Project.ProjectPrice),
            //        //ItemId = p.ItemId,
            //        //ItemName = p.PlanRefs.SelectMany(pp => pp.Project.Items.SelectMany(ppp => ppp.ItemName)),

            //    }) ;
            //}

            foreach (Item items in item)
            {
                data.Add(new CPlanViewModel()
                {
                    
                    ItemId = (int)items.ItemId,
                    ItemName = items.ItemName,
                  ProjectId= (int)items.ProjectId 
                  
                });
                
            }
            //-----------------------list轉datatable
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("planId"));
            dt.Columns.Add(new DataColumn("planName"));
            dt.Columns.Add(new DataColumn("PlanDescription"));
            dt.Columns.Add(new DataColumn("projectid"));
            dt.Columns.Add(new DataColumn("ProjectName"));
            dt.Columns.Add(new DataColumn("ProjectPrice"));
            dt.Columns.Add(new DataColumn("itemId"));
            dt.Columns.Add(new DataColumn("ItemName"));
            foreach (var t in data)
            {
                DataRow dr = dt.NewRow();

                dr["planId"] = t.PlanId;
                dr["PlanName"] = t.PlanName;
                dr["PlanDescription"] = t.PlanDescription;
                dr["projectid"] = t.ProjectId;
                dr["ProjectName"] = t.ProjectName + t.ItemName;
                dr["ProjectPrice"] = t.ProjectPrice;
                dr["itemId"] = t.ItemId;
                dr["ItemName"] = t.ItemName +$"{t.ItemId }";
                dt.Rows.Add(dr);
            }

          



            
            return View(dt);

            }
            else {
                return RedirectToAction("planComeparison");
            }
           

        }
        
      [HttpPost]
        public IActionResult planComeparisonTotal(int? planid)
        {//測試方案暫定planid=3

            var pl = _context.Plans.Where(p => p.PlanId == planid)
                .SelectMany(p => p.PlanRefs, (plan, project) => new { plan, project }).Where(p => p.project.PlanId == planid)
                .SelectMany(p => p.project.Project.Items, (prbg, it) => new { prbg.project.Project, it }).Where(p => p.Project.ProjectId == p.it.ProjectId)

                //.SelectMany(p => p.project.Project.Items, (projectid, item) => new { projectid, item }).Where(p => p.item.ProjectId == p.projectid.project.ProjectId)
                .Select(t => new
                {
                    planId = t.Project.PlanRefs.First().PlanId,
                    planName = t.Project.PlanRefs.First().Plan.PlanName,
                    projectid = t.Project.ProjectId,
                    ProjectName = (string)t.Project.ProjectName,
                    ProjectPrice = (double)t.Project.ProjectPrice,
                    itemId = t.it.ItemId,
                    ItemName = (string)t.it.ItemName,
                   
                }) ;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("planId"));
            dt.Columns.Add(new DataColumn("planName"));
           
            dt.Columns.Add(new DataColumn("projectid"));
            dt.Columns.Add(new DataColumn("ProjectName"));
            dt.Columns.Add(new DataColumn("ProjectPrice"));
            dt.Columns.Add(new DataColumn("itemId"));
            dt.Columns.Add(new DataColumn("ItemName"));
            foreach (var t in pl)
            {
                DataRow dr = dt.NewRow();

                dr["planId"] = t.planId;
                dr["PlanName"] = t.planName;
                
                dr["projectid"] = t.projectid;
                dr["ProjectName"] = t.ProjectName;
                dr["ProjectPrice"] = t.ProjectPrice;
                dr["itemId"] = t.itemId;
                dr["ItemName"] = t.ItemName ;
                dt.Rows.Add(dr);
            }


            
            if (dt != null)
            {string json=JsonSerializer.Serialize(pl);
                HttpContext.Session.SetString(CDictionary.SK_PLAN_COMPARERE_RESULT,json);
                
                return Json(json); }
            else
            {  return View(); }
           
        }

        public IActionResult PlanIntroductionProject(int? id)
        { //放方案介紹  固定item高度+男女差異+價格+修資料傳送型態(datatable)

            //vm方法
            List<CPlanViewModel> data = new List<CPlanViewModel>();
            var plan = from pl in _context.Plans.Where(p => p.PlanId == id)
                       select pl;

            foreach (Plan plans in plan)
            {
                data.Add(new CPlanViewModel()
                {
                    PlanName = plans.PlanName,
                    PlanDescription = plans.PlanDescription,
                    PlanId = plans.PlanId,

                });
                //data.Add(new CPlanViewModel() { plan=plans });
            }
            var project = from pj in _context.PlanRefs.Where(p => p.PlanId == id)
                          select pj;
            foreach (PlanRef projects in project)
            {
                data.Add(new CPlanViewModel()
                {
                    ProjectId = (int)projects.ProjectId,


                });
                //data.Add(new CPlanViewModel() { planRef=projects });
            }
            var item = from it in _context.Items.Include(i => i.Project).ThenInclude(i => i.PlanRefs.Where(i => i.PlanId == id))
                       select it;

            foreach (Item items in item)
            {
                data.Add(new CPlanViewModel()
                {
                    ItemId = (int)items.ItemId,
                    ItemName = items.ItemName,
                    //ProjectId = (int)items.ProjectId,
                    ProjectName = items.Project.ProjectName,
                    ProjectPrice = items.Project.ProjectPrice
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


        //[HttpPost]
        public IActionResult Reserve(IFormCollection item)
        { //預約總覽   尚未完成:日曆限制人數+第三方金流

            //var datas = (from s in _context.PlanRefs.Include(p=>p.Project).ThenInclude(p=>p.Items).ThenInclude(p=>p.)

            //             select s).Distinct();

            //var datas = (from s in _context.Plans.Include(p => p.PlanRefs).ThenInclude(p => p.Project).ThenInclude(p => p.Items)

            //             select s).Distinct();

            ViewBag.item = item["item"];
            ViewBag.mid = item["Mid"];
            ViewBag.pid = item["Pid"];
            //ViewBag.test = "test123";
            //ViewBag.item = item;

            return View();
        }
        public IActionResult Member()
        { //會員專項
            return View();
        }

        public IActionResult report(int id)
        {          //尚未完成: 補db去年資料+報告值差異比對+列印匯出功能
            ViewData["id"] = 55;
            var m = _context.Reserves.Where(s => s.MemberId == 46);

            _context.Members.Load();
            var j = (from s in _context.ReportDetails.Include(p => p.Item).Include(p => p.Report).ThenInclude(p => p.Reserve)
                     where s.Report.MemberId == 7
                     //select s.Report.Reserve.ReserveDate).Distinct();
                     select s);

            return View(j);
        }

        public IActionResult Customcompare()

        {//尚未完成:  測試比較後傳送資料
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
            var healthCheckItems = _context.Projects.Select(p => p.ProjectName).ToList();
            var healthCheckStatus = new List<string>();
            var waitStatus = new List<string>();

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_MEMBER_LOGIN))
            {
                string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
                Member? currentMember = JsonSerializer.Deserialize<Member>(json);
                int currentMemberId = currentMember?.MemberId ?? -1;

                foreach (var itemName in healthCheckItems)
                {
                    var healthReport = _context.HealthReports
                        .FirstOrDefault(hr => hr.MemberId == currentMemberId && hr.PlanId.HasValue);
                    healthCheckStatus.Add(healthReport != null ? "done" : "not done");
                }
            }
            else
            {
                var random = new Random();
                foreach (var itemName in healthCheckItems)
                {
                    healthCheckStatus.Add(random.Next(2) == 0 ? "done" : "not done");
                }
            }

            var randomWaitStatus = new Random();
            string[] possibleStatus = { "high", "medium", "low" };
            waitStatus.AddRange(healthCheckItems.Select(_ => possibleStatus[randomWaitStatus.Next(possibleStatus.Length)]));

            ViewBag.HealthCheckItems = healthCheckItems;
            ViewBag.HealthCheckStatus = healthCheckStatus;
            ViewBag.WaitStatus = waitStatus;

            // 取得每個 ProjectId 對應的 ItemName 列表
            var itemNamesByProjectId = new Dictionary<int, List<string>>();
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

            Console.WriteLine($"HealthCheckItems count: {healthCheckItems.Count}");
            Console.WriteLine($"HealthCheckStatus count: {healthCheckStatus.Count}");
            Console.WriteLine($"WaitStatus count: {waitStatus.Count}");

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


        public IActionResult testplan()
        { //plan 企業版
            return View();
        }

        public async Task<IActionResult> GetPlansAndProjects()
        {
            try
            {
                var plans = await _context.Plans
                    .Select(p => new
                    {
                        PlanId = p.PlanId,
                        PlanName = p.PlanName,
                        Projects = p.PlanRefs.Select(pr => pr.Project).ToList()
                    })
                    .ToListAsync();

                var projects = await _context.Projects
                    .Select(p => new { p.ProjectId, p.ProjectName })
                    .ToListAsync();

                var viewModel = new
                {
                    Plans = plans,
                    Projects = projects
                };

                return Json(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching plans and projects: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }



        public async Task<IActionResult> GetAllProjects()
        {//企業方案API2
            try
            {
                var projects = await _context.Projects
                    .Select(p => new { p.ProjectId, p.ProjectName })
                    .ToListAsync();

                return Json(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching projects: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}




