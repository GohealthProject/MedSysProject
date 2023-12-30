namespace MedSysProject.Models.BBL
{
    public class ShoppingCartManager
    {
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly MedSysContext _db;
        public ShoppingCartManager(IHttpContextAccessor httpContextAccessor, MedSysContext db)
        {
            _IHttpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public void AddToCart()
        {
            var context = _IHttpContextAccessor.HttpContext;
            if(context != null)
            {
                var data = context.Request.Form;
            }

            //List<CCartItem>? cart = null;
            //var q = _db.Products.Find(Int32.Parse(data["id"]));
            //string? json = "";
            //string? count = "";
            //if (HttpContext.Session.GetString(CDictionary.SK_ADDTOCART) != null)
            //{
            //    json = HttpContext.Session.GetString(CDictionary.SK_ADDTOCART);
            //    count = HttpContext.Session.GetString(CDictionary.SK_CARTLISTCOUNT);
            //    cart = JsonSerializer.Deserialize<List<CCartItem>>(json);
            //}
            //else
            //    cart = new List<CCartItem>();

            //CCartItem item = new CCartItem();
            //item.Product = q;
            //item.ProductName = q.ProductName;
            //item.UnitPrice = (int)((int)q.UnitPrice * 0.8);
            //item.小計 = Int32.Parse(data["count"]) * (int)((int)q.UnitPrice * 0.8);
            //item.count = Int32.Parse(data["count"]);
            //cart.Add(item);
            //count = cart.Count().ToString();
            //json = JsonSerializer.Serialize(cart);
            //HttpContext.Session.SetString(CDictionary.SK_ADDTOCART, json);
            //HttpContext.Session.SetString(CDictionary.SK_CARTLISTCOUNT, count);
            //return Ok();
        }
    }
}
