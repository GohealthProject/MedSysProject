using MedSysProject.Models.BBL;

namespace MedSysProject.Models
{
    public class EcPayModel
    {
        private readonly SessionHelper _sessionHelper;

        public string MerchantTradeNo { get; set; }
        public int total { get; set; }
        public string ProductID { get; set; }
        public string ProductCount { get; set; }
        public string ProductName { get; set; }
        public string MemberEmail { get; set; }
        public string WebSite { get; set; }


        public EcPayModel(SessionHelper sessionHelper)
        {
            _sessionHelper = sessionHelper;
        }

        public void EcPayLoadData()
        {
            List<CCartItem> cart = _sessionHelper.getCartList();
            if (cart.Count() == 0)
            {
                return;
            }
            MerchantTradeNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            foreach (CCartItem item in cart)
            {
                ProductID += item.Product.ProductId + "#";
                ProductName += item.Product.ProductName + "#";
                total += item.UnitPrice * item.count;
                ProductCount += item.count.ToString() + "#";
            }
            ProductID = ProductID.Substring(0, ProductID.Length - 1);
            MemberEmail = _sessionHelper.getSessionMember().MemberEmail;
            WebSite = $"https://localhost:7203/";
            total = total + (int)(total * 0.05);
        }
        public Dictionary<string, string> valueEcPay()
        {
            var orderGreen = new Dictionary<string, string>
                {
                    { "MerchantTradeNo",  MerchantTradeNo},
                    { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                    { "TotalAmount",  total.ToString()},
                    { "TradeDesc",  "無"},
                    { "ItemName", ProductName},
                    { "ExpireDate",  "3"},
                    { "CustomField1",  MemberEmail},
                    { "CustomField2",  ProductID},
                    { "CustomField3",  ProductCount},
                    { "CustomField4",  total.ToString()},
                    { "ReturnURL",  $"{WebSite}Shopping/paymethodinfo"},
                    { "OrderResultURL", $"{WebSite}Shopping/paySussess/{MerchantTradeNo}"},
                    { "MerchantID",  "2000132"},
                    { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
                    { "PaymentType",  "aio"},
                    { "ChoosePayment",  "ALL"},
                    { "EncryptType",  "1"},
                };
            return orderGreen;
        }
    }
}
