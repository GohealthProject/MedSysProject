using MimeKit.Cryptography;

namespace MedSysProject.Models.BBL
{
    public class OrderManager
    {
        private readonly SessionHelper _sessionHelper;
        private MedSysContext _context;
        public OrderManager (SessionHelper sessionHelper, MedSysContext context)
        {
            _sessionHelper = sessionHelper;
            _context = context;
        }
        public bool MidFindOrder(int Mid)
        {
            var order = _context.Orders.Where(n => n.MemberId == Mid);

            if(!order.Any())
            {
                return false;
            }
            return true;
        }

        public void CreateOrder(string MerchantTradeNo)
        {

            Order od = new Order();
            od.MemberId = _sessionHelper.getSessionMember().MemberId;
            od.OrderDate = DateTime.Now;
            od.ShipDate = DateTime.Now.AddDays(2);
            od.DeliveryDate = DateTime.Now.AddDays(5);
            od.StateId = 13;
            od.PayId = 1;
            od.ShipId = 2;
            od.MerchantTradeNo = MerchantTradeNo;
            _context.Orders.Add(od);
            
            _context.SaveChanges();
            CreateOrderDetail(MerchantTradeNo);
        }
        public void CreateOrderDetail(string MerchantTradeNo)
        {
            List<CCartItem> cart = _sessionHelper.getCartList();
            Order od = _context.Orders.Where(od=>od.MerchantTradeNo== MerchantTradeNo).FirstOrDefault();

            foreach (CCartItem cartItem in cart)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = od.OrderId;
                orderDetail.ProductId = cartItem.Product.ProductId;
                var CartProduct = _context.Products.Find(cartItem.Product.ProductId);
                CartProduct.UnitsInStock -= cartItem.count;
                orderDetail.Quantity = cartItem.count;
                orderDetail.UnitPrice = cartItem.UnitPrice;
                _context.OrderDetails.Add(orderDetail);
            }
            _context.SaveChanges();
        }
    }
}
