using System.ComponentModel;

namespace MedSysProject.Models
{
    public class MemberWarp
    {
        public MemberWarp() {
            _member = new Member();
        }
        private Member _member;
        public Member member { get { return this._member; } set {this. _member = value; } }
        public int MemberId { get { return this._member.MemberId; } set {this.member.MemberId=value; } }
        public bool  IsVerified { get { return this._member.IsVerified; } set { this.member.IsVerified = value; } }
        [DisplayName("會員姓名")]
        public string? MemberName { get { return this._member.MemberName; } set { this.member.MemberName = value; } }
        [DisplayName("會員性別")]
        public string? MemberGender { get { return this._member.MemberGender; } set { this.member.MemberGender = value; } }
        [DisplayName("會員生日")]
        public DateTime? MemberBirthdate { get { return this._member.MemberBirthdate; } set { this.member.MemberBirthdate = value; } }
        [DisplayName("會員手機")]
        public string MemberPhone { get { return this._member.MemberPhone; } set { this.member.MemberPhone = value; } }
        [DisplayName("會員信箱")]
        public string MemberEmail { get { return this._member.MemberEmail; } set { this.member.MemberEmail = value; } }
        [DisplayName("會員地址")]
        public string? MemberAddress { get { return this._member.MemberAddress; } set { this.member.MemberAddress = value; } }
        [DisplayName("會員密碼")]
        public string? MemberPassword { get { return this._member.MemberPassword; } set { this.member.MemberPassword = value; } }
        [DisplayName("暱稱")]
        public string? MemberNickname { get { return this._member.MemberNickname; } set { this.member.MemberNickname = value; } }
        [DisplayName("會員公司統編")]
        public int? TaxId { get { return this._member.TaxId; } set { this.member.TaxId = value; } }
        public string MemberImage { get { return this._member.MemberImage; } set { this.member.MemberImage = value; } }
        public int StatusId { get { return (int)this._member.StatusId; } set { this.member.StatusId = value; } }
        public string? VieifiedId { get { return this._member.VieifiedId; } set { this.member.VieifiedId = value; } }
        public int? MemberCityDistrictRoadId { get { return this._member.MemberCityDistrictRoadId; } set { this.member.MemberCityDistrictRoadId = value; } }
        public bool? GoogleLogin { get { return this._member.GoogleLogin; } set { this.member.GoogleLogin = value; } }
        public bool? FacebookLogin { get { return this._member.FacebookLogin; } set { this.member.FacebookLogin = value; } }
        public bool? LineLogin { get { return this._member.LineLogin; } set { this.member.LineLogin = value; } }
    }
}
