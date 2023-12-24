using MedSysProject.Hubs;
using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace MedSysProject.Controllers
{
    public class SignalRTestController : Controller
    {
        //注入
        private readonly IHubContext<ChatHub> _hubContext;
        private MedSysContext _db;
        private readonly IWebHostEnvironment _host;

        public SignalRTestController(MedSysContext db,IHubContext<ChatHub> hubContext, IWebHostEnvironment host)
        {
            _hubContext = hubContext;
            _db = db;
            _host = host;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChatList(int? MemberId,int? EmployeeId)
        {
            var member = _db.Members.Find(MemberId);
            var employee = _db.Employees.Find(EmployeeId);

            COnlineUser.onlinememberid = 0;
            COnlineUser.onlineemployeeid = 0;

            if (member == null && employee == null)
            {
                return NotFound();
            }
            else if (member != null && employee != null)
            {
                return NotFound();
            }
            else if (member != null)
            {
                COnlineUser.onlinememberid = (int)MemberId;
                ViewBag.MemberId = MemberId;
                return PartialView(member);
            }
            else if (employee != null)
            {
                COnlineUser.onlineemployeeid = (int)EmployeeId;
                ViewBag.EmployeeId = EmployeeId;
                return PartialView(employee);
            }

            return PartialView();
        }

        public IActionResult ChatRoom(int? roomid,int? MemberId, int? EmployeeId)
        {
            var room = _db.Rooms.Find(roomid);
            var member = _db.Members.Find(MemberId);
            var employee = _db.Employees.Find(EmployeeId);

            if (member == null && employee == null)
            {
                return NotFound();
            }
            else if (member != null && employee != null)
            {
                return NotFound();
            }
            else if (member != null)
            {
                ViewBag.MemberId = MemberId;
                return PartialView(member);
            }
            else if (employee != null)
            {
                ViewBag.EmployeeId = EmployeeId;
                return PartialView(employee);
            }

            return PartialView();
        }
    }
}
