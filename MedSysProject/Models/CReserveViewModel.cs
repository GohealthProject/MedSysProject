namespace MedSysProject.Models
{
    public class CReserve
    {
        public int ReserveId { get; set; }

        public int? MemberId { get; set; }

        public int? PlanId { get; set; }

        public DateTime? ReserveDate { get; set; }

        public string ReserveState { get; set; }

    }
}
