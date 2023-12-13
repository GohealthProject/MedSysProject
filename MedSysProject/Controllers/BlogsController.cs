﻿using MedSysProject.Models;
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

        //單篇貼文
        public IActionResult SinglePost(int? singleBlogID)
        {
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

            var q = _db.Comments.Where(n => n.BlogId ==singleBlogID&&n.ParentCommentId==null).Select(n => n.CommentId);
            ViewBag.CommentId = JsonSerializer.Serialize(q.ToList());
            return View(singlePost);
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
        public IActionResult Slider() 
        {

            return PartialView();
        }

        public IActionResult DemoTeachersCode() 
        {
            return View();
        }

        #region 載入留言相關
        /// <summary>
        /// 顯示留言
        /// </summary>
        /// <param name="BlogId"></param>
        /// <returns></returns>
        public IActionResult ShowComments(int BlogId)
        {
            var mainComments = (_db.Comments.Include(comment => comment.Member)
                                            .Include(comment => comment.Employee)
                                            .Include(comment => comment.Employee.EmployeeClass)
                                            .Where(comment => comment.BlogId == BlogId && comment.ParentCommentId == null)
                                .Select(comment => comment)).ToList();
            return PartialView(mainComments);
        }
        public IActionResult ShowReplies(int mainCommentId) 
        {
            List<Comment> allReplies = new List<Comment>();
            howManyExactly(mainCommentId, ref allReplies);
            return PartialView("ShowReplies", allReplies);
        }
        /// <summary>
        /// 執行遞迴檢查的方法
        /// </summary>
        /// <param name="parentCommentId"></param>
        /// <param name="allReplies"></param>
        private void howManyExactly(int parentCommentId,ref List<Comment> allReplies) 
        {
            var subReplies = _db.Comments.Include(reply => reply.Member)
                                         .Include(reply => reply.Employee)
                                         .Include(reply=>reply.Employee.EmployeeClass)
                                         .Include(reply => reply.ParentComment)
                                         .Include(reply => reply.ParentComment.Employee)
                                         .Include(reply=>reply.ParentComment.Employee.EmployeeClass)
                                         .Include(reply => reply.ParentComment.Member)
                                         .Where(reply => reply.ParentCommentId == parentCommentId).ToList();
            foreach (var reply in subReplies) 
            { 
                allReplies.Add(reply);
                howManyExactly(reply.CommentId, ref allReplies);
            }             
        }
        #endregion
    }
}
