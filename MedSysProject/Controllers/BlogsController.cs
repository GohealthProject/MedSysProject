using Microsoft.AspNetCore.Mvc;

namespace MedSysProject.Controllers
{
    public class BlogsController : Controller
    {
        /// <summary>
        /// 主畫面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult SinglePost() 
        {
            return View();
        }
        /// <summary>
        /// 可能需要接收類別參數??
        /// </summary>
        /// <returns></returns>
        public IActionResult ViewCategory() 
        {
            return View();
        }

        public IActionResult AboutUs() 
        {
            return View();
        }
        public IActionResult ContactUs() 
        {
            return View();
        }

        public IActionResult Test() 
        {
            return View();
        }
        public IActionResult Create() 
        {
            return View();
        }

    }
}
