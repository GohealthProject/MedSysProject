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
        public IActionResult TestList() 
        {
            IEnumerable<Blog> blogs = from blog in _db.Blogs
                                      .Include(blog=>blog.Employee)
                                      .Include(blog=>blog.ArticleClass)
                                      select blog;
            return View(blogs);
        }
        public IActionResult TestTinyMCE() 
        {
            return View();
        }
        /// <summary>
        /// 好像不能偷雞摸狗，參數名稱要與asp-for一致
        /// </summary>
        /// <param name="newBlog"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult TestTinyMCE(string Title,int ArticleClassId,string Content, IFormFile BlogImage,int EmployeeId) 
        {
            Blog newBlog = new Blog();
            newBlog.Title = Title;
            newBlog.ArticleClassId = ArticleClassId;
            newBlog.Views = 0;
            newBlog.Content = Content;
            //已經二進位了嗎??
            //IFromFile怎麼轉二進位寫入資料庫??
            //newBlog.BlogImage = BlogImage;
            newBlog.EmployeeId= EmployeeId;
            return RedirectToAction("TestList");
        }

    }
}
