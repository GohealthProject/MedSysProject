using System.ComponentModel;

namespace MedSysProject.Models
{
    public class MemberWarp
    {
        public int MemberId { get; set; }
        [DisplayName("會員姓名")]
        public string? MemberName { get; set; }
        [DisplayName("會員性別")]
        public string? MemberGender { get; set; }
        [DisplayName("會員生日")]
        public DateTime? MemberBirthdate { get; set; }
        [DisplayName("會員手機")]
        public string MemberPhone { get; set; } = null!;
        [DisplayName("會員信箱")]
        public string MemberEmail { get; set; } = null!;
        [DisplayName("會員地址")]
        public string? MemberAddress { get; set; }
        [DisplayName("會員密碼")]
        public string? MemberPassword { get; set; }
        [DisplayName("會員公司統編")]
        public int? TaxId { get; set; }
    }
}
