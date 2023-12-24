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
            var member = _db.Members.Find(MemberId);
            var employee = _db.Employees.Find(EmployeeId);
            var room = _db.RoomRefs.Where(e => e.Employeeid == employee.EmployeeId && e.Memberid == member.MemberId).Select(r => r.Roomid).FirstOrDefault();
            if (room == null || room == 0)
            {
                //新增房間
                var room1 = new Room();
                room1.RoomName = member.MemberName + "和" + employee.EmployeeName + "的聊天室";
                _db.Rooms.Add(room1);
                _db.SaveChanges();

                var roomref = new RoomRef();
                roomref.Roomid = room1.RoomId;
                roomref.Memberid = member.MemberId;
                roomref.Employeeid = employee.EmployeeId;
                _db.RoomRefs.Add(roomref);
                _db.SaveChanges();

                room = room1.RoomId;
            }

            if (member == null && employee == null)
            {
                return NotFound();
            }
            else if (member != null && employee != null)
            {
                ViewBag.MemberName = member.MemberName;
                ViewBag.EmployeeName = employee.EmployeeName;
                ViewBag.MemberId = MemberId;
                ViewBag.EmployeeId = EmployeeId;
                ViewBag.RoomId = room;

                //灌聊天紀錄
                var chat = _db.Messages.Where(r => r.RoomId == room).OrderBy(d => d.SendTime).ToList();

                return PartialView(chat);
            }

            return PartialView();
        }
    }
}
