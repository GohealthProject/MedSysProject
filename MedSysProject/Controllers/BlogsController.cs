﻿using Microsoft.AspNetCore.Mvc;

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
        public IActionResult IndexOld() 
        {
            return View();
        }

        

    }
}
