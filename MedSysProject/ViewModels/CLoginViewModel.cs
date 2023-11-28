using MedSysProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MedSysProject.ViewModels
{
    public class CLoginViewModel
    {
        public string txtEmail { get; set; }
        public string txtPassWord { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}
