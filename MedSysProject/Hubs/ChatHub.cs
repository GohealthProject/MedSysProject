using MedSysProject.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MedSysProject.Hubs
{
    public class ChatHub : Hub
    {
        private MedSysContext _db;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHub(MedSysContext db, IHubContext<ChatHub> hubContext)
        {
            _db = db;
            _hubContext = hubContext;
        }

        public static List<int> MemberIDList = new List<int>();
        public static List<int> EmployeeIDList = new List<int>();
        public static List<string> ConnectionIDList = new List<string>();
        int memberID;
        int employeeID;

        public override async Task OnConnectedAsync()
        {

            var userisMember = _db.Members.FirstOrDefault(u => u.MemberId == COnlineUser.onlinememberid);
            var userisEmployee = _db.Employees.FirstOrDefault(u => u.EmployeeId == COnlineUser.onlineemployeeid);
            if (userisMember != null || userisEmployee != null)
            {
                if (userisMember != null && userisEmployee == null) //用戶是會員
                {
                    if (!MemberIDList.Contains(userisMember.MemberId))
                        MemberIDList.Add(userisMember.MemberId);

                    ConnectionIDList.Add(Context.ConnectionId);
                    memberID = userisMember.MemberId;

                    //更新連線ID列表
                    userisMember.ConnectionId = Context.ConnectionId;
                    _db.SaveChanges();

                    //string json = JsonConvert.SerializeObject(member);
                    //await _hubContext.Clients.All.SendAsync("UpdateMemberList", member.MemberId, member.MemberName, json);

                    string Empjson = "";
                    foreach (var item in EmployeeIDList) //將員工列表轉成json格式
                    {
                        var id = _db.Employees.Find(item);
                        var employeeidandname = new { id.EmployeeId, id.EmployeeName,id.EmployeeConnectionId };
                        Empjson += JsonConvert.SerializeObject(employeeidandname);
                        if (item != EmployeeIDList.Last())
                            Empjson += ",";
                    }
                    Empjson = "[" + Empjson + "]";

                    string Memberjson = "";
                    foreach (var item in MemberIDList) //將會員列表轉成json格式
                    {
                        var id = _db.Members.Find(item);
                        var memberidandname = new { id.MemberId, id.MemberName,id.ConnectionId };
                        Memberjson += JsonConvert.SerializeObject(memberidandname);
                        if (item != MemberIDList.Last())
                            Memberjson += ",";
                    }
                    Memberjson = "[" + Memberjson + "]";

                    //string json = JsonConvert.SerializeObject(employeeidandname);
                    await _hubContext.Clients.All.SendAsync("UpdateEmployeeList", Empjson);

                    //await _hubContext.Clients.All.SendAsync("UpdateMemberList", Memberjson);


                }
                else if (userisMember == null && userisEmployee != null) //用戶是員工
                {
                    if (!EmployeeIDList.Contains(userisEmployee.EmployeeId))
                    {
                        EmployeeIDList.Add(userisEmployee.EmployeeId);
                    }

                    ConnectionIDList.Add(Context.ConnectionId);
                    employeeID = userisEmployee.EmployeeId;

                    //更新連線ID列表
                    userisEmployee.EmployeeConnectionId = Context.ConnectionId;
                    _db.SaveChanges();

                    string Empjson = "";
                    foreach (var item in EmployeeIDList)
                    {
                        var id = _db.Employees.Find(item);
                        var employeeidandname = new { id.EmployeeId, id.EmployeeName };
                        Empjson += JsonConvert.SerializeObject(employeeidandname);
                        if (item != EmployeeIDList.Last())
                            Empjson += ",";
                    }
                    Empjson = "[" + Empjson + "]";

                    string Memberjson = "";
                    foreach (var item in MemberIDList)
                    {
                        var id = _db.Members.Find(item);
                        var memberidandname = new { id.MemberId, id.MemberName,id.ConnectionId };
                        Memberjson += JsonConvert.SerializeObject(memberidandname);
                        if (item != MemberIDList.Last())
                            Memberjson += ",";
                    }
                    Memberjson = "[" + Memberjson + "]";

                    //string json = JsonConvert.SerializeObject(employeeidandname);
                    //await _hubContext.Clients.All.SendAsync("UpdateEmployeeList", Empjson);

                    await _hubContext.Clients.All.SendAsync("UpdateMemberList", Memberjson);

                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userisMember = _db.Members.FirstOrDefault(u => u.MemberId == COnlineUser.onlinememberid);
            var userisEmployee = _db.Employees.FirstOrDefault(u => u.EmployeeId == COnlineUser.onlineemployeeid);
            if (userisMember != null || userisEmployee != null)
            {
                if (userisMember != null && userisEmployee == null)
                {
                    MemberIDList.Remove(userisMember.MemberId);
                    ConnectionIDList.Remove(Context.ConnectionId);
                    memberID = userisMember.MemberId;

                    //更新連線ID列表
                    userisMember.ConnectionId = null;
                    _db.SaveChanges();

                    //string json = JsonConvert.SerializeObject(member);
                    //await _hubContext.Clients.All.SendAsync("UpdateMemberList", member.MemberId, member.MemberName, json);


                }
                else if (userisMember == null && userisEmployee != null)
                {
                    var empconnection = _db.Employees.FirstOrDefault(e => e.EmployeeConnectionId == Context.ConnectionId);

                    //EmployeeIDList.Remove(empconnection.EmployeeId);
                    //ConnectionIDList.Remove(Context.ConnectionId);
                    //employeeID = empconnection.EmployeeId;



                    //更新連線ID列表
                    userisEmployee.EmployeeConnectionId = null;
                    _db.SaveChanges();

                    string json = "";
                    foreach (var item in EmployeeIDList)
                    {
                        var id = _db.Employees.Find(item);
                        var employeeidandname = new { id.EmployeeId, id.EmployeeName,id.EmployeeConnectionId };
                        json += JsonConvert.SerializeObject(employeeidandname);
                        if (item != EmployeeIDList.Last())
                            json += ",";
                    }
                    json = "[" + json + "]";

                    //string json = JsonConvert.SerializeObject(employeeidandname);
                    await _hubContext.Clients.All.SendAsync("UpdateEmployeeList", json);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(int? roomid,int? memberid,int? employeeid,string message)
        {
            //先判斷送出訊息的是誰，是會員還是員工
            var member = _db.Members.Find(memberid);
            var employee = _db.Employees.Find(employeeid);

            var room = roomid;

            if (member != null && employee == null) //會員
            {
                //找出該會員的連線ID
                var memberconnection = _db.Members.Where(m => m.MemberId == member.MemberId).Select(c => c.ConnectionId).FirstOrDefault();

                //找出該會員的房間ID
                //var roomidlist = _db.RoomRefs.Where(r => r.Memberid == member.MemberId).Select(r => r.Roomid).ToList();

                //找出該會員的房間ID中，有沒有包含該員工的房間ID
                //var roomidlist2 = _db.RoomRefs.Where(r => r.Employeeid == employee.EmployeeId).Select(r => r.Roomid).ToList();

                //找出該會員的房間ID中，有沒有包含該員工的房間ID
                //var roomidlist3 = roomidlist.Intersect(roomidlist2).ToList();

                //如果有包含，就把該房間ID指定給room
                //if (roomidlist3.Count() > 0)
                //{
                //    room = roomidlist3[0];
                //}
                //else
                //{
                //    //如果沒有包含，就新增一筆房間資料
                //    var newroom = new Room();
                //    newroom.RoomName = member.MemberName + "和" + employee.EmployeeName + "的聊天室";
                //    _db.Rooms.Add(newroom);
                //    _db.SaveChanges();

                //    //新增房間參考資料
                //    var newroomref = new RoomRef();
                //    newroomref.Roomid = newroom.RoomId;
                //    newroomref.Memberid = member.MemberId;
                //    newroomref.Employeeid = employee.EmployeeId;
                //    _db.RoomRefs.Add(newroomref);
                //    _db.SaveChanges();

                //    room = newroom.RoomId;
                //}

                //找出該房間的所有訊息
                //var messagelist = _db.Messages.Where(m => m.RoomId == room).ToList();

                //新增訊息
                var newmessage = new Message();
                newmessage.RoomId = room;
                if (member != null)
                    newmessage.MemberId = member.MemberId;
                if (employee != null)
                    newmessage.EmployeeId = employee.EmployeeId;
                newmessage.MessageContent = message;
                newmessage.SendTime = DateTime.Now;
                _db.Messages.Add(newmessage);
                _db.SaveChanges();

                //將訊息傳送給該房間的所有人
                await _hubContext.Clients.Group(room.ToString()).SendAsync("ReceiveMessage", member.MemberName, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            }

            else if (member == null && employee != null) //員工
            {
                //找出該員工的連線ID
                var employeeconnection = _db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).Select(c => c.EmployeeConnectionId).FirstOrDefault();

                //找出該會員的房間ID
                //var roomidlist = _db.RoomRefs.Where(r => r.Memberid == member.MemberId).Select(r => r.Roomid).ToList();

                //找出該會員的房間ID中，有沒有包含該員工的房間ID
                //var roomidlist2 = _db.RoomRefs.Where(r => r.Employeeid == employee.EmployeeId).Select(r => r.Roomid).ToList();

                //找出該會員的房間ID中，有沒有包含該員工的房間ID
                //var roomidlist3 = roomidlist.Intersect(roomidlist2).ToList();

                //如果有包含，就把該房間ID指定給room
                //if (roomidlist3.Count() > 0)
                //{
                //    room = roomidlist3[0];
                //}
                //else
                //{
                //    //如果沒有包含，就新增一筆房間資料
                //    var newroom = new Room();
                //    newroom.RoomName = member.MemberName + "和" + employee.EmployeeName + "的聊天室";
                //    _db.Rooms.Add(newroom);
                //    _db.SaveChanges();

                //    //新增房間參考資料
                //    var newroomref = new RoomRef();
                //    newroomref.Roomid = newroom.RoomId;
                //    newroomref.Memberid = member.MemberId;
                //    newroomref.Employeeid = employee.EmployeeId;
                //    _db.RoomRefs.Add(newroomref);
                //    _db.SaveChanges();

                //    room = newroom.RoomId;
                //}

                //找出該房間的所有訊息
                //var messagelist = _db.Messages.Where(m => m.RoomId == room).ToList();

                //新增訊息
                var newmessage = new Message();
                
                newmessage.RoomId = room;
                if (member != null)
                    newmessage.MemberId = member.MemberId;
                if (employee != null)
                    newmessage.EmployeeId = employee.EmployeeId;
                newmessage.MessageContent = message;
                newmessage.SendTime = DateTime.Now;
                _db.Messages.Add(newmessage);
                _db.SaveChanges();

                //將訊息傳送給該房間的所有人
                await _hubContext.Clients.Group(room.ToString()).SendAsync("ReceiveMessage", employee.EmployeeName, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            }
        }
    }
}