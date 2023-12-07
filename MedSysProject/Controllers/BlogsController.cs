using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using TinifyAPI;

namespace MedSysProject.Controllers
{
    public class BlogsController : Controller
    {
        private readonly MedSysContext _db;

        public BlogsController(MedSysContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 主畫面，可能要改寫成Ajax?
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {//要放什麼過去?
            //能不能裝在List當中
            //測試能不能傳送List
            //最近的文章
            var post = (from blog in _db.Blogs
                       .Include(blog => blog.Employee)
                       .Include(blog => blog.ArticleClass)
                       .OrderByDescending(blog => blog.BlogId)
                       .Take(7)
                        select blog).ToList();//全部文章類別 新->舊
            //活動快訊最新文章
            var activity = from blog in _db.Blogs
                            .Include(blog => blog.Employee)
                            .Include(blog => blog.ArticleClass)
                            .Where(blog => blog.ArticleClass.BlogClassId == 1)
                            .OrderByDescending(blog => blog.BlogId)
                            .Take(4)
                           select blog;

            foreach (var blog in activity) { post.Add(blog); }
            //醫療新知最新文章
            var medical = from blog in _db.Blogs
                          .Include(blog => blog.Employee)
                          .Include(blog => blog.ArticleClass)
                          .Where(blog => blog.ArticleClass.BlogClassId == 2)
                          .OrderByDescending(blog => blog.BlogId)
                          .Take(4)
                          select blog;
            foreach (var blog in medical) { post.Add(blog); }
            //名人分享會最新文章
            var celebrity = from blog in _db.Blogs
                            .Include(blog => blog.Employee)
                            .Include(blog => blog.ArticleClass)
                            .Where(blog => blog.ArticleClass.BlogClassId == 3)
                            .OrderByDescending(blog => blog.BlogId)
                            .Take(8)
                            select blog;
            foreach (var blog in celebrity) { post.Add(blog); }

            #region 把圖片縮小反而比較久
            //foreach (var blog in post)
            //{
            //    var source = Tinify.FromBuffer(blog.BlogImage);
            //    var resized = await source.Resize(new
            //    {
            //        method = "fit",
            //        width = 545,
            //        height = 342,
            //    });
            //    blog.BlogImage = await resized.ToBuffer();
            //}
            #endregion
            return View(post);
        }

        public IActionResult SinglePost(int? BlogID)
        {
            return View();
        }
        public IActionResult SelectBlogCategory(int? CategoryID, int page = 1)
        {//
         //var q = _db.BlogCategories.Where(c => c.BlogClassId == CategoryID);
         //var q = _db.Blogs.Include(e => e.Employee).Include(e => e.ArticleClass.BlogCategory1).Where(c => c.ArticleClassId == CategoryID);
            IEnumerable<Blog> q = null;

            int pageSize = 5;
            int total;

            if (CategoryID == null)
            {
                total = _db.Blogs.Count();

                q = _db.Blogs.Include(e => e.Employee)
                    .Include(e => e.ArticleClass)
                    .OrderByDescending(e => e.BlogId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                //q = from blog in _db.Blogs.Include(e => e.Employee)
                //    .Include(e => e.ArticleClass)
                //    select blog;

                ViewBag.Cate = "";
            }

            else
            {
                total = _db.Blogs.Include(e => e.Employee)
                    .Include(e => e.ArticleClass)
                    .Where(c => c.ArticleClassId == CategoryID)
                    .Count();

                q = _db.Blogs.Include(e => e.Employee)
                    .Include(e => e.ArticleClass)
                    .Where(c => c.ArticleClassId == CategoryID)
                    .OrderByDescending(e => e.BlogId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);


                ViewBag.Cate = "分類：" + (q.ToList())[0].ArticleClass.BlogCategory1;
            }

            int maxPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
            if (page < 1) page = 1;
            if (page > maxPage) page = maxPage;
            ViewBag.Page = page;
            ViewBag.MaxPage = maxPage;
            ViewBag.total = total;
            ViewBag.pagesize = pageSize;
            ViewBag.CategoryID = CategoryID;

            return View(q);
        }

        public IActionResult GetBlogImageByte(int? id)
        {
            Blog emp = _db.Blogs.Find(id);
            byte[]? img = emp?.BlogImage;

            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }

        public IActionResult GetEmpImageByte(int? id)
        {
            Employee emp = _db.Employees.Find(id);
            byte[]? img = emp?.EmployeePhoto;

            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }
        /// <summary>
        /// 測試partial View用
        /// </summary>
        /// <returns></returns>
        public IActionResult Index2() 
        {
            return View();
        }
        public IActionResult Slider(int id) 
        {
            var b = _db.Blogs.Find(id);

            return PartialView(b);
        }
        public IActionResult ad (int id)
        {
             var 最新的文章 = _db.Blogs.Where(n=>n.EmployeeId == id).OrderByDescending(n=>n.BlogId).Take(5).Select(n=>n.BlogId);
            var 最多觀看的 = _db.Blogs.Where(n => n.EmployeeId == id).OrderByDescending(n => n.Views).Take(5).Select(n => n.BlogId);

            List<int> ne = 最新的文章.ToList();
            List<int> top5 = 最多觀看的.ToList();
            ViewBag.new5Array = JsonSerializer.Serialize(ne);
            ViewBag.View5 = JsonSerializer.Serialize(top5);

            return View();
        }

    }
}
