using Microsoft.AspNetCore.Http;
using System.Text.Json;
namespace MedSysProject.Models.BBL
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        
        public SessionHelper(IHttpContextAccessor httpContextAccessor) 
        {
            _IHttpContextAccessor = httpContextAccessor;
        }

        public MemberWarp getSessionMember()
        {
            var context = _IHttpContextAccessor.HttpContext;
            if (context != null)
            {
                string? LoginMemberJson = context.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
                if(LoginMemberJson != null)
                {
                    return JsonSerializer.Deserialize<MemberWarp>(LoginMemberJson);
                }
            }
            return null;
        }
        public void setCartList(List<CCartItem> cart)
        {
            var context = _IHttpContextAccessor.HttpContext;
            if(context != null && context.Session!=null)
            {
                string? CartListJson = JsonSerializer.Serialize(cart);
                context.Session.SetString(CDictionary.SK_ADDTOCART, CartListJson);
            }
        }
        public void setCartCount(string count)
        {
            var context = _IHttpContextAccessor.HttpContext;
            if(context != null &&context.Session != null)
            {
                context.Session.SetString(CDictionary.SK_CARTLISTCOUNT, count);
            }
        }
        public List<CCartItem> getCartList()
        {
            var context = _IHttpContextAccessor.HttpContext;
            string? CartListJson = context.Session.GetString(CDictionary.SK_ADDTOCART) ?? "";

            if (string.IsNullOrEmpty(CartListJson))
            {
                return new List<CCartItem>();
            }
                
            return JsonSerializer.Deserialize<List<CCartItem>>(CartListJson);
                

        }
        public string getCartCount()
        {
            var context = _IHttpContextAccessor.HttpContext;

                string? CartCountJson = context.Session.GetString(CDictionary.SK_CARTLISTCOUNT)??"";
                if(string.IsNullOrEmpty(CartCountJson))
                {
                return "";
                }

            return CartCountJson;
        }
    }
}
