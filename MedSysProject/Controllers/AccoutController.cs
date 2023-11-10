using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class AccoutController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel c)
        {
            if (c == null)
                return View("Login");

            MedSysContext db = new MedSysContext();
            var q = db.Members.FirstOrDefault(n => n.MemberEmail == c.txtEmail);
            if (q != null)
            {
                if (q.MemberPassword == c.txtPassWord)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
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
            MedSysContext db = new MedSysContext();
            Member newMenber = new Member();
            newMenber.MemberGender = vm.MemberGender;
            newMenber.MemberAddress = vm.MemberAddress;
            newMenber.MemberBirthdate = vm.MemberBirthdate;
            newMenber.MemberEmail = vm.MemberEmail;
            newMenber.MemberName = vm.MemberName;
            newMenber.MemberPhone = vm.MemberPhone;
            newMenber.MemberPassword = vm.MemberPassword;
            db.Members.Add(newMenber);
            db.SaveChanges();
            ViewBag.Sussess = true;
            return RedirectToAction("Login");
        }

    }
}
