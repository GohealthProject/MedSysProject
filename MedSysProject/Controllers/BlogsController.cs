using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult IndexOld()
        {//要放什麼過去?
            //能不能裝在List當中
            //測試能不能傳送List
            var post = (from blog in _db.Blogs
                       .Include(blog => blog.Employee)
                       .Include(blog => blog.ArticleClass)
                       .OrderByDescending(blog => blog.BlogId)
                       .Take(7)
                        select blog).ToList();//全部文章類別 新->舊
            var activity = from blog in _db.Blogs
                            .Include(blog => blog.Employee)
                            .Include(blog => blog.ArticleClass)
                            .Where(blog => blog.ArticleClass.BlogClassId == 1)
                            .OrderByDescending(blog => blog.BlogId)
                            .Take(4)
                           select blog;

            foreach (var blog in activity) { post.Add(blog); }
            var medical = from blog in _db.Blogs
                          .Include(blog => blog.Employee)
                          .Include(blog => blog.ArticleClass)
                          .Where(blog => blog.ArticleClass.BlogClassId == 2)
                          .OrderByDescending(blog => blog.BlogId)
                          .Take(4)
                          select blog;
            foreach (var blog in medical) { post.Add(blog); }
            var celebrity = from blog in _db.Blogs
                            .Include(blog => blog.Employee)
                            .Include(blog => blog.ArticleClass)
                            .Where(blog => blog.ArticleClass.BlogClassId == 3)
                            .OrderByDescending(blog => blog.BlogId)
                            .Take(9)
                            select blog;
            foreach (var blog in celebrity) { post.Add(blog); }
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

    }
}
