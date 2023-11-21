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
        private readonly IWebHostEnvironment _host;
        public AccoutController(MedSysContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        public IActionResult MemberCenter()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_MEMBER_LOGIN))
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }//會員中心

        public IActionResult Login()
        {
            return View();
        } //登入
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
        } //登入
        public IActionResult UpdataMember()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_MEMBER_LOGIN))
                return RedirectToAction("Login");
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
            return View(m);
        } //修改會員
        [HttpPost]
        public IActionResult UpdataMember(MemberWarp m,IFormFile fileN)
        {
            string webPath = Path.Combine(_host.WebRootPath, "img\\MemberImg", fileN.FileName);
            using (var fileStream = new FileStream(webPath, FileMode.Create))
            {
                fileN.CopyTo(fileStream);
            }
            Member? Upm = _db.Members.FirstOrDefault(n=>n.MemberId == m.MemberId);
            Upm.MemberImage = fileN.FileName;
            Upm.MemberEmail= m.MemberEmail;
            Upm.MemberName= m.MemberName;
            Upm.MemberBirthdate = m.MemberBirthdate;
            Upm.MemberAddress= m.MemberAddress;
            Upm.MemberNickname = m.MemberNickname;
            _db.SaveChanges();
            return RedirectToAction("MemberCenter", "Accout");
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
        public IActionResult VerifyemailPassword()
        {
            string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp nown = new MemberWarp();
            nown.member = JsonSerializer.Deserialize<Member>(json);
            return View(nown);
        }
        [HttpPost]
        public IActionResult VerifyemailPassword(MemberWarp nown)
        {
            var Upm = _db.Members.Find(nown.MemberId);
            Upm.MemberPassword = nown.MemberPassword;
            _db.SaveChanges();
            return RedirectToAction("MemberCenter", "Accout");
        }
        public IActionResult MemberState()
        {
            string json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp upm = new MemberWarp();
            upm.member = JsonSerializer.Deserialize<Member>(json);
            return View(upm);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }

}
