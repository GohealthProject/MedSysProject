using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSysProject.Controllers
{
    public class Test : Controller
    {
        private MedSysContext _db;
        private readonly IWebHostEnvironment _host;
        /// <summary>
        /// 入珠寫法 why??
        /// </summary>
        /// <param name="db"></param>
        /// <param name="host"></param>
        public Test(MedSysContext db,IWebHostEnvironment host) 
        {
            _db = db;
            _host = host;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            IEnumerable<Product> datas = null;
            //MedSysContext db =new MedSysContext();
            //datas = db.Products.Select(n => n);
            return View(datas);

        }
        public IActionResult Blog() 
        {
            return View();
        }

        public IActionResult BlogsIncludeTest() 
        { 
            var blogs = from blog in _db.Blogs
                        .Include(author=>author.Employee)
                        select blog;
            return View(blogs);
        }

    }
}
