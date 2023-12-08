namespace MedSysProject.ViewModels
{
    public class CPlanProjectItems
    {
       
        public int PlanId { get; set; }

        public string PlanName { get; set; }

        public string PlanDescription { get; set; }
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
        public double? ProjectPrice { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }
    }
}
