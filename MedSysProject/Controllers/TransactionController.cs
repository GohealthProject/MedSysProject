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

namespace MedSysProject.Controllers
{
    public class TransactionController : Controller
    {
        
        private readonly MedSysContext _context;
        public TransactionController(MedSysContext medSysContext)
        {
            
            _context = medSysContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult members()
        {
            //IEnumerable<ReportDetail> datas = null;
            //List<CReportWrap> datas2 = null;
            //datas2 = new CReportWrap().Report();

            var datas = from s in _context.Members.Include(p => p.HealthReports)

                        select s;
            //_context.Members.Load();
            //var datass = _context.Members.Include(q => q.HealthReports).SelectMany(p => new {p,p.HealthReports});
                         

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

            Reserve rsggg = JsonSerializer.Deserialize<Reserve>(rs2);
            //_context.Add(rsggg);
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
        { "TotalAmount",  "1450"},//交易金額
        { "TradeDesc",  "Test"},//交易描述
        { "ItemName",  "測試商品"},//商品名稱
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
                ecpayOrder.RtnMsg = "已付款";
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
            

            //var data = new Dictionary<string, string>();
            //foreach (string key in id.Keys)
            //{
            //    data.Add(key, id[key]);
            //}

            //string temp = id["MerchantTradeNo"]; //寫在LINQ(下一行)會出錯，
            //var ecpayOrder = _context.EcpayOrders.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
            //if (ecpayOrder != null)
            //{
            //    ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
            //    if (id["RtnMsg"] == "Succeeded") 
            //        ecpayOrder.RtnMsg = "已付款";
            //    //ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
            //    //ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
            //    _context.SaveChanges();
            //}
            return View("EcpayView", data);
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

