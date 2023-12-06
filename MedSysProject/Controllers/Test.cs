using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
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
        public Test(MedSysContext db, IWebHostEnvironment host)
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

        #region 模擬部落格後台

        /// <summary>
        /// 文章列表
        /// </summary>
        /// <returns></returns>
        public IActionResult TestList() 
        {
            IEnumerable<Blog> blogs = from blog in _db.Blogs
                                      .Include(blog=>blog.Employee)
                                      .Include(blog=>blog.ArticleClass)
                                      select blog;
            return View(blogs);
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <returns></returns>
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
        //------------------------------------
        //edit跟新增一樣都要寫兩個
        //------------------------------------
        /// <summary>
        /// 文章列表點擊執行，id指定修改哪篇文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult TestEdit(int? id) 
        {
            Blog blog = _db.Blogs.FirstOrDefault(n => n.BlogId == id);
            if (blog == null) { return RedirectToAction("TestList"); }
            else { return View(blog); }
        }
        /// <summary>
        /// 按下submit把修改後的資料傳送到這裡
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult TestEdit(int BlogId,string Title,int ArticleClassId,string Content,IFormFile BlogImage,int EmployeeId)
        {
            Blog original = _db.Blogs.FirstOrDefault(n => n.BlogId == BlogId);
            if (original != null) 
            { 
                original.Title = Title;
                original.ArticleClassId = ArticleClassId;
                original.Content = Content;
                if (BlogImage != null&&BlogImage.Length>0) 
                {
                    using (MemoryStream ms = new MemoryStream()) 
                    { 
                        BlogImage.CopyTo(ms);
                        original.BlogImage=ms.ToArray();
                    }
                }
                _db.SaveChanges();

            }
            
            return RedirectToAction("TestList");
        }
        /// <summary>
        /// 刪除文章，刪除之前=>留言要先處理
        /// </summary>
        /// <returns></returns>
        public IActionResult TestDelete(int? id) 
        {
            if (id == null) { return RedirectToAction("TestList"); }
            else 
            { //找出該篇所有留言
                var comments = from comment in _db.Comments
                               where comment.BlogId == id
                               select comment;
                foreach (var comment in comments) 
                { //刪除留言
                    this._db.Comments.Remove(comment);
                }
                Blog iWantDelete = _db.Blogs.FirstOrDefault(blog=>blog.BlogId == id);
                _db.Blogs.Remove(iWantDelete);
                _db.SaveChanges();
            }
            return RedirectToAction("TestList");
        }
        public IActionResult TestTiny2() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TestTiny2(string Title, int ArticleClassId, string Content, IFormFile BlogImage, int EmployeeId)
        {
            try
            {
                Blog newBlog = new Blog();
                newBlog.Title = Title;
                newBlog.ArticleClassId = ArticleClassId;
                newBlog.Views = 0;
                newBlog.Content = Content;
                newBlog.CreatedAt = DateTime.Now;
                byte[] afterCompressed = null;
                if (BlogImage != null && BlogImage.Length > 0)
                {
                    using (MemoryStream originalMs = new MemoryStream())
                    {
                        BlogImage.CopyTo(originalMs);
                        var imageCompressionService = new ImageCompressionService("rhkRy28T0Xz4JDdQ1y45cbxNTW47Gm46");
                        afterCompressed = await imageCompressionService.CompressImageAsync(originalMs.ToArray());
                    }
                }
                newBlog.BlogImage = afterCompressed;
                newBlog.EmployeeId= EmployeeId;
                _db.Blogs.Add(newBlog);
                await _db.SaveChangesAsync();

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return RedirectToAction("TestList");
        }

        #endregion

        #region 前台顯示
        /// <summary>
        /// 測試顯示單篇文章
        /// </summary>
        /// <returns></returns>
        public IActionResult SinglePost(int? singleBlogID)
        {
            //singleBlogID = 37;
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
        #endregion
    }

    internal class ImageCompressionService
    {
        private readonly string apiKey;
        private readonly HttpClient client;

        public ImageCompressionService(string apiKey)
        {
            this.apiKey = apiKey;
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("Authorization",$"Bear{apiKey}");
        }
        public async Task<byte[]> CompressImageAsync(byte[] imageData)
        {
            try
            {
                using (var content = new ByteArrayContent(imageData)) 
                {
                    var response = await client.PostAsync("https://api.tinify.com/shrink", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var compressedStream = await response.Content.ReadAsStreamAsync();
                        using (var ms = new MemoryStream())
                        {
                            await compressedStream.CopyToAsync(ms);
                            return ms.ToArray();
                        }
                    }
                    else 
                    {
                        Console.WriteLine($"Failed to compress image. Status code: {response.StatusCode}");
                        return null;
                    }
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error while compressing image: {ex.Message}");
                return null;
            }
        }
    }
}
