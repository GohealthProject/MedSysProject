using MedSysProject.Models;

namespace MedSysProject.ViewModels
{
    public class CPlansProjectsViewModel
    {
        public IEnumerable<Plan>? Plans { get; set; }
        public IEnumerable<Project>? Projects { get; set; }
    }
}
