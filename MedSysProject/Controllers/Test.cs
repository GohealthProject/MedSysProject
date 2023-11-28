using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Protocol.Plugins;

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
            TempData["SuccessMessage"] = "";
            try
            {
                Blog newBlog = new Blog();
                newBlog.Title = Title;
                newBlog.ArticleClassId = ArticleClassId;
                newBlog.Views = 0;//開局瀏覽次數為0
                newBlog.Content = Content;
                newBlog.CreatedAt = DateTime.Now;
                //已經二進位了嗎??
                //IFromFile怎麼轉二進位寫入資料庫??
                //newBlog.BlogImage = BlogImage;
                if (BlogImage != null && BlogImage.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        BlogImage.CopyTo(ms);
                        newBlog.BlogImage = ms.ToArray();
                    }
                }
                else { newBlog.BlogImage = null; }
                newBlog.EmployeeId = EmployeeId;//先寫死，如果新增成功的話作者應該都是1號James
                _db.Blogs.Add(newBlog);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "部落格已成功新增！";
            }
            catch (Exception ex) 
            {
                TempData["SuccessMessage"] = ex.Message;
            }
            
            return RedirectToAction("TestList");

        }
        /// <summary>
        /// 測試顯示單篇文章
        /// </summary>
        /// <returns></returns>
        public IActionResult SinglePost(int? singleBlogID) 
        {
            singleBlogID = 34;
            IEnumerable<Blog> singlePost = null;
            if (singleBlogID != null)
            {
                singlePost = (from blog in _db.Blogs.Include(blog => blog.ArticleClass)
                                          .Include(blog => blog.Employee)
                                          .Where(blog => blog.BlogId == singleBlogID)
                             select blog).ToList();
            }
            else 
            {
                 singlePost = (from blog in _db.Blogs.Include(blog => blog.ArticleClass)
                                                   .Include(blog => blog.Employee)
                                                   .OrderByDescending(blog => blog.BlogId).Take(1)
                                                   select blog).ToList();
            }
            
            
            return View(singlePost);
        }

    }
}
