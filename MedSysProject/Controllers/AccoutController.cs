using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace MedSysProject.Controllers
{
    public class AccoutController : Controller
    {
        private MedSysContext _db = null;
        public AccoutController(MedSysContext db) { 
        _db= db;
        }
        public IActionResult MemberCenter()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel c)
        {
            if (c == null)
                return View("Login");

            var q = _db.Members.FirstOrDefault(n => n.MemberEmail == c.txtEmail);
            if (q != null)
            {
                if (q.MemberPassword == c.txtPassWord)
                {
                    string json = JsonSerializer.Serialize(q);
                    HttpContext.Session.SetString(CDictionary.SK_MEMBER_LOGIN, json);
                    return RedirectToAction("MemberCenter", "Accout");
                }
            }
            return View();
        }
        public IActionResult UpdataMember()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_MEMBER_LOGIN))
                return RedirectToAction("Login");
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
            return View(m);
        }
        [HttpPost]
        public IActionResult UpdataMember(MemberWarp m)
        {
            
            Member? Upm = _db.Members.FirstOrDefault(n=>n.MemberId == m.MemberId);
            Upm.MemberEmail= m.MemberEmail;
            Upm.MemberPassword= m.MemberPassword;
            Upm.MemberName= m.MemberName;
            Upm.MemberGender = m.MemberGender;
            Upm.MemberBirthdate = m.MemberBirthdate;
            Upm.MemberAddress= m.MemberAddress;
            Upm.MemberNickname = m.MemberNickname;
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(MemberWarp vm)
        {
            if (vm == null)
                return View();
            
            if (_db.Members.FirstOrDefault(n => n.MemberEmail == vm.MemberEmail) == null)
            {
                _db.Members.Add(vm.member);
                _db.SaveChanges();
                ViewBag.Sussess = true;
                MemberWarp nowm  = new MemberWarp();
                nowm.member = _db.Members.OrderBy(n => n.MemberId).LastOrDefault();
                string json = JsonSerializer.Serialize(nowm);
                HttpContext.Session.SetString(CDictionary.SK_MEMBER_LOGIN, json);
                return RedirectToAction("Verifyemail");
            }
            return RedirectToAction("Register");
        }
        public IActionResult Verifyemail()
        {
            if(HttpContext.Session.Keys.Contains(CDictionary.SK_MEMBER_LOGIN))
            return View();
            return RedirectToAction("Login");
        }
        public IActionResult test()
        {
            return View();
        }
    }
}
