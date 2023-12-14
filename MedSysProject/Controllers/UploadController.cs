using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Construction;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedSysProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _host;

        public UploadController(IWebHostEnvironment host)
        {
            _host = host;
        }


        // GET: api/<UploadController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UploadController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UploadController>
        [HttpPost]
        public IActionResult Post()
        {
            var fail = Request.Form;
            
            IFormFile file = Request.Form.Files[0];


            //upload image 







            //string webPath = Path.Combine(_host.WebRootPath, "img\\MemberImg", fileN.FileName);
            //using (var fileStream = new FileStream(webPath, FileMode.Create))
            //{
            //    fileN.CopyTo(fileStream);
            //}

            return Ok();

        }

        // PUT api/<UploadController>/5
        [HttpPut("Up/")]
        public IActionResult Puttt()
        {
            var fail = Request.Form;
            //string webPath = Path.Combine(_host.WebRootPath, "img\\MemberImg", fileN.FileName);
            //using (var fileStream = new FileStream(webPath, FileMode.Create))
            //{
            //    fileN.CopyTo(fileStream);
            //}

            return Ok();
        }

        // PUT api/<UploadController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UploadController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
