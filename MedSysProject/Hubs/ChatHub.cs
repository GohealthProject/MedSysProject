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
                if (userisMember != null && userisEmployee == null)
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
                else if (userisMember == null && userisEmployee != null)
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

                    string json = "";
                    foreach (var item in EmployeeIDList)
                    {
                        var id = _db.Employees.Find(item);
                        var employeeidandname = new { id.EmployeeId, id.EmployeeName };
                        json += JsonConvert.SerializeObject(employeeidandname);
                        if (item != EmployeeIDList.Last())
                            json += ",";
                    }
                    json = "[" + json + "]";

                    //string json = JsonConvert.SerializeObject(employeeidandname);
                    await _hubContext.Clients.All.SendAsync("UpdateEmployeeList", json);

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

                    EmployeeIDList.Remove(empconnection.EmployeeId);
                    ConnectionIDList.Remove(Context.ConnectionId);
                    employeeID = empconnection.EmployeeId;



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

        public async Task SendMessage(string userId, string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, user, message);
        }
    }
}