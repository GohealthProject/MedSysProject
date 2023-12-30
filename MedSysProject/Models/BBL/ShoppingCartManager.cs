namespace MedSysProject.Models.BBL
{
    public class ShoppingCartManager
    {
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly MedSysContext _db;
        private readonly SessionHelper _sessionHelper;
        public ShoppingCartManager(IHttpContextAccessor httpContextAccessor, MedSysContext db, SessionHelper sessionHelper)
        {
            _IHttpContextAccessor = httpContextAccessor;
            _db = db;
            _sessionHelper = sessionHelper;
        }
        public MemberWarp getMember()
        {
            return _sessionHelper.getSessionMember();
        }
        public void AddToCart()
        {
            var context = _IHttpContextAccessor.HttpContext;

            if(context != null)
            {
                var data = context.Request.Form;

                if (!int.TryParse(data["id"],out int MemberID))
                {
                    return;
                }

                var q = _db.Products.Find(MemberID);
                List<CCartItem> cart = _sessionHelper.getCartList();
                string? count = _sessionHelper.getCartCount();

                CCartItem item = new CCartItem();
                item.Product = q;
                item.ProductName = q.ProductName;
                item.UnitPrice = (int)((int)q.UnitPrice * 0.8);
                item.小計 = Int32.Parse(data["count"]) * (int)((int)q.UnitPrice * 0.8);
                item.count = Int32.Parse(data["count"]);
                cart.Add(item);

                count = cart.Count().ToString();

                _sessionHelper.setCartCount(count);
                _sessionHelper.setCartList(cart);
            }
        }
        public void RemoveCart(int id)
        {
            List<CCartItem> cart = _sessionHelper.getCartList();
            string count = _sessionHelper.getCartCount();
            if (cart.Count() == 0)
            {
                return;
            }
            foreach(CCartItem item in cart)
            {
                if(item.Product.ProductId == id)
                {
                    cart.Remove(item);
                    break;
                }
            }
            count = cart.Count().ToString();
            _sessionHelper.setCartList(cart);
            _sessionHelper.setCartCount(count);
        }
        public string getCartCount()
        {
            return _sessionHelper.getCartCount();
        }
        public void changeQta(int pid , int qta)
        {
            List<CCartItem> cart = _sessionHelper.getCartList();

            foreach(CCartItem item in cart)
            {
                if(item.Product.ProductId == pid)
                {
                    item.count= qta;
                    item.小計 = (int)item.Product.UnitPrice * item.count;
                    break;
                }
            }
            _sessionHelper.setCartList(cart);
        }
        
    }
}
