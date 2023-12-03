using Google.Apis.Auth;
using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
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
        public IActionResult xxx()
        {//plan選擇，個人版，男女區別，待更改CONTROLL名稱==>自訂方案(含搜尋項目功能)

            return View(_context.Projects);
        }
        public IActionResult planComeparison()
        {////方案比較(設計filter篩選方案)
            return View();
        }
        public IActionResult planComeparisonTotal()
        {////方案比較總攬(總項+PDF產生)
            return View();
        }

      public IActionResult PlanIntroductionProject(int? id)
        { //project區==>放方案介紹
            var project = _context.Plans.Where(p => p.PlanId == id);
            var join = from p in project
                       from pj in _context.Projects.DefaultIfEmpty()
                       from it in _context.Items.DefaultIfEmpty()
                       select new
                       {
                           p.PlanId,
                           p.PlanName,
                           p.PlanDescription,
                           p.PlanRefs,
                           pj.ProjectId,
                           pj.ProjectName,
                           pj.ProjectPrice,
                           it.ItemId,
                           it.ItemName,

                       };
            //_context.Plans.ToList();
            //_context.Products.ToList();
          

         
            return View(join);
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






    }
}



