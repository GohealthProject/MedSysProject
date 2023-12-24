using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static System.Net.WebRequestMethods;
using MedSysProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using Google.Apis.Http;

namespace MedSysProject.Controllers
{
    public class TransactionController : Controller
    {
        
        private readonly MedSysContext _context;
        System.Net.Http.IHttpClientFactory _httpClientFactory;
        public TransactionController(MedSysContext medSysContext , System.Net.Http.IHttpClientFactory httpClientFactory)
        {
            //_httpClientFactory = httpClientFactory;
            _context = medSysContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult checkreserve(string date)
        {
           
            return View();
        }
            public IActionResult editAll()
        {
            var form = Request.Form;
            var did = form["detailid"];
            string id = form["detailid"];
            string result = form["result"];
            var temp = id.Split(',');
            var temp2 = result.Split(",");
            for (int i = 0;i<= id.Split(',').Length-1;i++)
            {
                
                int t = int.Parse(temp[i]);
                ReportDetail rd = _context.ReportDetails.Where(p => p.ReportDetailId.Equals(t)).FirstOrDefault();
                rd.Result = temp2[i];
            }
            _context.SaveChanges();

            return View();
        }
            public IActionResult editreport()
        {
            var form = Request.Form;
            var rpdi = form["reportdetailid"];
            var rpi = form["reportid"];
            var item = form["itemname"];
            var rs = form["result"];
            var edit = form["editinput"];
            

            var id = int.Parse(rpdi);
            var items = _context.ReportDetails.Where(p => p.ReportDetailId.Equals(id)).FirstOrDefault();
            items.Result = rs;
            //ReportDetail rd = new ReportDetail();
            //rd.ReportDetailId = int.Parse(rpdi);
            //rd.ReportId = int.Parse(rpi);
            //rd.Result = rs;

            //_context.Add();
            _context.SaveChanges();

            return View();
        }


        public IActionResult members(int id)
        {
          
            IQueryable<Member> datas = null;
            
            if (id == 0)
            {
                 datas = from s in _context.Members.Include(p => p.HealthReports)
                            select s;
            }
            else {
                 datas =from s in _context.Members.Include(p => p.HealthReports).ThenInclude(p=>p.Reserve)
                        where s.MemberId == id
                           select s;
                    }
        

            return Json(datas);

        }
        public IActionResult rp2(int id)
        {

            var rp2json = _context.ReportDetails.Where(p => p.ReportId == id);
                     
            return Json(rp2json);

        }


        [HttpPost]
        public IActionResult reserve()
        {
            var form = Request.Form;
            
            var rs2 = form["reserve"];
            var rss = form["reservesub"];
            var htrp = form["healthreport"];
            var rpdt = form["reportdetail"];

            //Reserve rsggg = JsonSerializer.Deserialize<Reserve>(rs2);
            //ReservedSub rsb = JsonSerializer.Deserialize<ReservedSub>(rss);
            //HealthReport hrp = JsonSerializer.Deserialize<HealthReport>(htrp);
            //ReportDetail rpd = JsonSerializer.Deserialize<ReportDetail>(rpdt);
            //_context.Add(rsggg);
            //_context.Add(rsb);
            //_context.Add(hrp);
            //_context.Add(rpd);
            //_context.SaveChanges();
            return Content("rsyes");
        }
        [HttpPost]
        public IActionResult reservesub()
        {
            var form = Request.Form;
            var rss = form["reservesub"];
            ReservedSub rsb =JsonSerializer.Deserialize<ReservedSub>(rss);
            //_context.Add(rsb);
            //_context.SaveChanges();
            return Content("rsbyes");
        }
        [HttpPost]
        public IActionResult healthreport()
        {
            //var items = Request.Form;

            //List<int> itemlist = items["itemID"];

            //var q = _context.Items.Where(n => itemlist.Contains(n.ItemId));

            //var re = _context.Reserves.OrderByDescending(n => n.ReserveId).FirstOrDefault().ReserveId;

            //foreach(var item in q)
            //{
            //    ReservedSub r = new ReservedSub();
            //    r.Reserved = re;
            //    r.Item = item;
            //}
            var form = Request.Form;
            var htrp = form["healthreport"];
            HealthReport hrp = JsonSerializer.Deserialize<HealthReport>(htrp);

            //_context.Add(hrp);
            //_context.SaveChanges();
            return Content("hrpyes");
        }
        [HttpPost]
        public IActionResult reportdetail()
        {
            var form = Request.Form;
            var rpdt = form["reportdetail"];
            ReportDetail hrp = JsonSerializer.Deserialize<ReportDetail>(rpdt);
            //_context.Add(hrp);
            //_context.SaveChanges();
            return Content("rdlyes");
        }

        public IActionResult payment()
        {
            //step1 : 網頁導入傳值到前端
            //
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //需填入你的網址
            var website = $"https://localhost:7203";
            var order = new Dictionary<string, string>
    {
        //綠界需要的參數

        //必填
        { "MerchantID",  "3002599"},//特店編號
        { "MerchantTradeNo",  orderId},//特店訂單編號 不可重複使用
        { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},//特店交易時間
        { "PaymentType",  "aio"},//交易類型(固定aio)
        { "TotalAmount",  "28250"},//交易金額
        { "TradeDesc",  "健檢預約"},//交易描述
        { "ItemName",  "活力青年方案 "},//商品名稱
        { "ReturnURL",  $"{website}/Transaction/addOrders"},//付款完成通知回傳網址
        { "ChoosePayment",  "ALL"},//選擇預設付款方式
        { "EncryptType",  "1"},//CheckMacValue加密類型

        //選填
        /*{ "ExpireDate",  "3"},*///分期
        { "CustomField1",  ""},//自訂名稱欄位1
        { "CustomField2",  ""},
        { "CustomField3",  ""},
        { "CustomField4",  ""},
        { "OrderResultURL", $"{website}/Transaction/payInfo/{orderId}"},//Client端回傳付款結果網址
        //{ "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
        //{ "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
        { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},//隱藏付款方式
        
        
        
    };
            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);
            return View(order);
            //string neworder = JsonSerializer.Serialize(order);
            
              
            //return Json(neworder);
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


        //step4 : 新增訂單
        [HttpPost]
        [Route("Transaction/AddOrders")]
        public string addOrders(EcpayOrder json)
        {


            string num = "0";
            //try
            //{

            //    EcpayOrder Orders = new EcpayOrder();
            //    Orders.MemberId = json.MemberId;
            //    Orders.MerchantTradeNo = "12";
            //    Orders.RtnCode = 0; //未付款
            //    Orders.RtnMsg = "訂單成功尚未付款";
            //    //Orders.TradeNo = json.MemberId.ToString();
            //    //Orders.TradeAmt = json.TradeAmt;
            //    //Orders.PaymentDate = Convert.ToDateTime(json.MerchantTradeDate);
            //    //Orders.PaymentType = json.PaymentType;
            //    //Orders.PaymentTypeChargeFee = "0";
            //    //Orders.TradeDate = json.MerchantTradeDate;
            //    /// Orders.SimulatePaid = 0;
            //    _context.EcpayOrders.Add(Orders);
            //    _context.SaveChanges();
                num = "OK";
            //}
            //catch (Exception ex)
            //{
            //    num = ex.ToString();
            //}
            return num;
        }

        [HttpPost]
        public ActionResult payInfo(IFormCollection id)
        {
            
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp m = System.Text.Json.JsonSerializer.Deserialize<MemberWarp>(json);
            var rs = _context.Reserves.Where(p=>p.MemberId == m.MemberId).OrderBy(p=>p.ReserveId).LastOrDefault();
            var hr = _context.HealthReports.Where(p => p.MemberId == m.MemberId).OrderBy(p => p.ReserveId).LastOrDefault();
            var data = new Dictionary<string, string>();
            foreach (string key in id.Keys)
            {
                data.Add(key, id[key]);
            }

            string temp = id["MerchantTradeNo"]; //寫在LINQ(下一行)會出錯，
            EcpayOrder ecpayOrder = new EcpayOrder();

            ecpayOrder.MerchantTradeNo = temp;
            ecpayOrder.MemberId = "00";
            ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
            if (id["RtnMsg"] == "Succeeded")
            {
                rs.PaymentStatus = 1;
                hr.Paymentstatus = 1;
;                ecpayOrder.RtnMsg = "已付款";
                _context.Reserves.Add(rs);
                _context.HealthReports.Add(hr);
                _context.SaveChanges();
            }
            else {
                ecpayOrder.RtnMsg = "未付款";
            }
            ecpayOrder.TradeNo = id["TradeNo"];
            ecpayOrder.TradeAmt = int.Parse(id["TradeAmt"]);
            ecpayOrder.TradeDate = id["TradeDate"];

            ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
            ecpayOrder.PaymentType = id["PaymentType"];
            ecpayOrder.PaymentTypeChargeFee = id["PaymentTypeChargeFee"];
            ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
            _context.EcpayOrders.Add(ecpayOrder);
            _context.SaveChanges();
            return RedirectToAction("Accout/Index");//MemberCenter
           
        }

      

        public static string EmailText(string TradeNo, string proname, string proCount, string total)
        {
            string html = "";
            List<string> proList = new List<string>();
            List<string> proCountList = new List<string>();
            proCountList = proCount.Split('#').ToList();
            total = Int32.Parse(total).ToString("N0");
            proList = proname.Split('#').ToList();
            List<Product> products = new List<Product>();

            html = "<h2>你好！很高興您能來我們網站消費。</h2>";
            html += "<h3>您的EcPay交易編號為：" + TradeNo + "</h3>";
            html += "<table style='border-collapse:collapse;border:1px solid #ddd'><thead><tr style='border:1px solid #ddd;padding:8px;'><td style='border:1px solid #ddd;padding:8px;'>產品名稱</td><td style='padding:8px;'>數量</td></tr><thead><tbody>";
            for (int i = 0; i < proList.Count - 1; i++)
            {
                html += "<tr style='border:1px solid #ddd;padding:8px;'><td style='border:1px solid #ddd;padding:8px;'>" + "產品名稱" + "</td><td style='padding:8px;'>" + 1 + "</td></tr>";
            }
            html += "<tr style='border:1px solid #ddd;padding:8px;'><td style='border:1px solid #ddd;padding:8px;'>總價格:<td style='padding:8px;'> " + total + "元<td></tr>";
            html += "</tbody></table>";
            html += "期待你能回到我們網站再次消費，謝謝！<br />";
            html += "MedSys團隊敬上";
            return html;
        }


        /// step5 : 取得虛擬帳號 資訊
        //[HttpPost]
        //public ActionResult AccountInfo(FormCollection id)
        //{
        //    var data = new Dictionary<string, string>();
        //    foreach (string key in id.Keys)
        //    {
        //        data.Add(key, id[key]);
        //    }

        //    string temp = id["MerchantTradeNo"]; //寫在LINQ會出錯
        //    var ecpayOrder = _context.EcpayOrders.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
        //    if (ecpayOrder != null)
        //    {
        //        ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
        //        if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
        //        //ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
        //        //ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
        //        _context.SaveChanges();
        //    }
        //    return View("EcpayView", data);
        //}

 

    }
}

