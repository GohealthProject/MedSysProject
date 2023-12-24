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

        public static List<int> EmployeeIDList = new List<int>();

        /// <summary>
        /// 連線事件
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {


            //看Session，SK_EMPLOYEE_LOGIN有哪些員工在線上
            var employee = JsonConvert.DeserializeObject<Employee>(Context.GetHttpContext().Session.GetString(CDictionary.SK_EMPLOYEE_LOGIN));
            if (employee != null)
            {
                //如果有，就加入EmployeeIDList
                EmployeeIDList.Add(employee.EmployeeId);
                //並且將員工的連線ID，加入到Groups中
                await Groups.AddToGroupAsync(Context.ConnectionId, employee.EmployeeId.ToString());

                //將Groups導入到前端
                await _hubContext.Clients.All.SendAsync("RefreshEmployeeOnlineList", EmployeeIDList);

            }

            //如果中途有員工上線，就將該員工的名字和ID，加入到聊天列表中
            await _hubContext.Clients.All.SendAsync("AddEmployee", employee.EmployeeId, employee.EmployeeName);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            //看Session，SK_EMPLOYEE_LOGIN有哪些員工在線上
            var employee = JsonConvert.DeserializeObject<Employee>(Context.GetHttpContext().Session.GetString(CDictionary.SK_EMPLOYEE_LOGIN));
            if (employee != null)
            {
                //如果有，就將該員工的連線ID，從Groups中移除
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, employee.EmployeeId.ToString());
                //並且將員工從EmployeeIDList中移除
                EmployeeIDList.Remove(employee.EmployeeId);
            }

            //如果中途有員工離線，就將該員工的名字和ID，從聊天列表中移除
            await _hubContext.Clients.All.SendAsync("RemoveEmployee", employee.EmployeeId, employee.EmployeeName);

            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendMessage(string userId,string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, user, message);
        }
    }
}