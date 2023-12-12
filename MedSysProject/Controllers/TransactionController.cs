using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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

        public IActionResult rp2(int id)
        {

            var rp2json = _context.ReportDetails.Where(p => p.ReportId == id);
                     
            return Json(rp2json);

        }

        public IActionResult payment()
        {
            //step1 : 網頁導入傳值到前端

            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //需填入你的網址
            var website = $"https://localhost:7203";
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
            try
            {

                EcpayOrder Orders = new EcpayOrder();
                Orders.MemberId = "1";
                Orders.MerchantTradeNo = "10";
                Orders.RtnCode = 0; //未付款
                Orders.RtnMsg = "訂單成功尚未付款";
                //Orders.TradeNo = json.MerchantID.ToString();
                //Orders.TradeAmt = json.TotalAmount;
                //Orders.PaymentDate = Convert.ToDateTime(json.MerchantTradeDate);
                //Orders.PaymentType = json.PaymentType;
                //Orders.PaymentTypeChargeFee = "0";
                //Orders.TradeDate = json.MerchantTradeDate;
                //Orders.SimulatePaid = 0;
                _context.EcpayOrders.Add(Orders);
                _context.SaveChanges();
                num = "OK";
            }
            catch (Exception ex)
            {
                num = ex.ToString();
            }
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
            var ecpayOrder = _context.EcpayOrders.Where(m => m.MerchantTradeNo == temp).FirstOrDefault();
            if (ecpayOrder != null)
            {
                ecpayOrder.RtnCode = int.Parse(id["RtnCode"]);
                if (id["RtnMsg"] == "Succeeded") ecpayOrder.RtnMsg = "已付款";
                //ecpayOrder.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
                //ecpayOrder.SimulatePaid = int.Parse(id["SimulatePaid"]);
                _context.SaveChanges();
            }
            return View(data);
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

