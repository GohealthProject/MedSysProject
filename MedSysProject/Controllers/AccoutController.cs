using Google.Apis.Auth;
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

        string GAccountName = "";
        string GAccountEmail = "";

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
            MemberWarp m = new MemberWarp();
            m.member = q;
            if (q != null)
            {
                if (m.member.MemberPassword == c.txtPassWord && m.IsVerified)
                {
                    string json = JsonSerializer.Serialize(q);
                    HttpContext.Session.SetString(CDictionary.SK_MEMBER_LOGIN, json);
                    return RedirectToAction("MemberCenter", "Accout");
                }
                else if (m.member.MemberPassword == c.txtPassWord && !m.IsVerified)
                {
                    return RedirectToAction("Verifyemail");
                }
                else
                {
                    ViewBag.Msg = "<i class=\"fas fa-times\"></i> 電子信箱或密碼錯誤";
                    return View();
                }
            }
            ViewBag.Msg = "<i class=\"fas fa-times\"></i> 電子信箱或密碼錯誤";
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
        public IActionResult UpdataMember(MemberWarp m, IFormFile fileN)
        {
            string webPath = Path.Combine(_host.WebRootPath, "img\\MemberImg", fileN.FileName);
            using (var fileStream = new FileStream(webPath, FileMode.Create))
            {
                fileN.CopyTo(fileStream);
            }
            Member? Upm = _db.Members.FirstOrDefault(n => n.MemberId == m.MemberId);
            Upm.MemberImage = fileN.FileName;
            Upm.MemberEmail = m.MemberEmail;
            Upm.MemberName = m.MemberName;
            Upm.MemberBirthdate = m.MemberBirthdate;
            Upm.MemberAddress = m.MemberAddress;
            Upm.MemberNickname = m.MemberNickname;
            _db.SaveChanges();

            string json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            MemberWarp? om = JsonSerializer.Deserialize<MemberWarp>(json);
            var q = _db.Members.Where(n => n.MemberId == m.MemberId).FirstOrDefault();
            om.member = q;
            json = JsonSerializer.Serialize(om);
            HttpContext.Session.SetString(CDictionary.SK_MEMBER_LOGIN, json);

            return RedirectToAction("MemberCenter", "Accout");
        }
        public IActionResult Register()
        {

            ViewBag.GAccountEmail = TempData["GAccountEmail"];
            ViewBag.GAccountName = TempData["GAccountName"];

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
                MemberWarp nowm = new MemberWarp();
                nowm.member = _db.Members.OrderBy(n => n.MemberId).LastOrDefault();
                string json = JsonSerializer.Serialize(nowm);
                HttpContext.Session.SetString(CDictionary.SK_MEMBER_LOGIN, json);
                return RedirectToAction("Verifyemail");
            }
            return RedirectToAction("Register");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(MemberWarp vm)
        {
            if (vm == null)
                return View();

            if (_db.Members.FirstOrDefault(n => n.MemberEmail == vm.MemberEmail) != null)
            {
                MemberWarp nowm = new MemberWarp();
                nowm.MemberEmail = vm.MemberEmail;
                string json = JsonSerializer.Serialize(nowm.MemberEmail);

                HttpContext.Session.SetString(CDictionary.SK_FPWD_SUBMIT, json);
                return RedirectToAction("VerifyemailforgerPwd");
            }

            ViewBag.Msg = "查無此帳號，請重新輸入。";
            return View();
        }

        public IActionResult VerifyemailforgerPwd()
        {
            //寄信
            Guid g = Guid.NewGuid();
            string s = g.ToString().Replace("-", "");

            HttpContext.Session.SetString(CDictionary.SK_FPWD_VERIFY, s);
            ViewBag.ver = s;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_FPWD_SUBMIT))
            {
                ViewBag.MemberEmail = JsonSerializer.Deserialize<string>(HttpContext.Session.GetString(CDictionary.SK_FPWD_SUBMIT));
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult ver(string key)
        {
            if (key == null)
            {
                TempData["expired"] = "<div class=\"rounded rounded-3 bg-warning text-dark p-3 mb-2\"><i class=\"fas fa-exclamation-triangle\"></i> 連結已失效，請再試一次。</div>";
                return RedirectToAction("Login");
            }

            string keys = HttpContext.Session.GetString(CDictionary.SK_MEMBER_VERIFY);

            if (key == keys)
            {
                //database is verifed = true
                var q = _db.Members.FirstOrDefault(n => n.MemberEmail == HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN));
                q.IsVerified = true;
                _db.SaveChanges();

                return RedirectToAction("UpdataMember");


            }
            else
            {
                TempData["expired"] = "<div class=\"rounded rounded-3 bg-warning text-dark p-3 mb-2\"><i class=\"fas fa-exclamation-triangle\"></i> 連結已失效，請再試一次。</div>";
                return RedirectToAction("Login");
            }
        }

        public IActionResult forgetpwdver(string key)
        {
            if (key == null)
                return RedirectToAction("Login");

            string keys = HttpContext.Session.GetString(CDictionary.SK_FPWD_VERIFY);

            if (key == keys)
            {
                return RedirectToAction("ResetPassword");
            }
            else
            {
                TempData["expired"] = "<div class=\"rounded rounded-3 bg-warning text-dark p-3 mb-2\"><i class=\"fas fa-exclamation-triangle\"></i> 連結已失效，請再試一次。</div>";
                return RedirectToAction("Login");
            }
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(CResetPasswordViewModel vm)
        {
            //修改密碼
            if (vm == null)
                return View();

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_FPWD_SUBMIT) && vm.newPwdChk == vm.newPwd)
            {
                //todo 這裡要修復
                var q = _db.Members.FirstOrDefault(n => n.MemberEmail == HttpContext.Session.GetString(CDictionary.SK_FPWD_SUBMIT));
                q.MemberPassword = vm.newPwd;
                _db.SaveChanges();

                TempData["resetLogin"] = "<div class=\"rounded rounded-3 bg-success text-light p-3 mb-2\"><i class=\"fas fa-check-circle\"></i> 密碼修改成功，請重新登入。</div>";
                
                return RedirectToAction("Login");
            }
            TempData["expired"] = "<div class=\"rounded rounded-3 bg-warning text-dark p-3 mb-2\"><i class=\"fas fa-exclamation-triangle\"></i> 連結已失效，請再試一次。</div>";
            return RedirectToAction("Login");
        }

        public IActionResult Verifyemail()
        {
            //寄信
            Guid g = Guid.NewGuid();
            string s = g.ToString().Replace("-", "");

            HttpContext.Session.SetString(CDictionary.SK_MEMBER_VERIFY, s);
            ViewBag.ver = s;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_MEMBER_LOGIN))
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

        /// <summary>
        /// 驗證 Google 登入授權
        /// </summary>
        /// <returns></returns>
        public IActionResult ValidGoogleLogin()
        {
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
            if (payload == null)
            {
                // 驗證失敗
                ViewData["Msg"] = "驗證 Google 授權失敗";
            }
            else
            {
                IEnumerable<Member> q = null;

                if (_db.Members.FirstOrDefault(n => n.MemberEmail == payload.Email) == null)
                {
                    TempData["GAccountName"] = payload.Name.ToString();
                    TempData["GAccountEmail"] = payload.Email.ToString();

                    return RedirectToAction("Register");
                }
                else
                {
                    var qa = _db.Members.FirstOrDefault(n => n.MemberEmail == payload.Email);
                    string json = JsonSerializer.Serialize(qa);
                    HttpContext.Session.SetString(CDictionary.SK_MEMBER_LOGIN, json);

                    return RedirectToAction("MemberCenter", "Accout");
                }



                //驗證成功，取使用者資訊內容
                ViewData["Msg"] = "驗證 Google 授權成功" + "<br>";
                ViewData["Msg"] += "Email:" + payload.Email + "<br>";
                ViewData["Msg"] += "Name:" + payload.Name + "<br>";
                ViewData["Msg"] += "Picture:" + payload.Picture;
            }



            return View();
        }

        /// <summary>
        /// 驗證 Google Token
        /// </summary>
        /// <param name="formCredential"></param>
        /// <param name="formToken"></param>
        /// <param name="cookiesToken"></param>
        /// <returns></returns>
        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return payload;
        }

        public IActionResult MenberQA()
        {
            return View();
        }


        public IActionResult MemberLive()
        {
            return View();
        }
    }

}
