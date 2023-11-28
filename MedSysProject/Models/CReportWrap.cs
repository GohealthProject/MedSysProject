using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Formats.Asn1.AsnWriter;

namespace MedSysProject.Models
{
    public partial class CReportWrap
    {
        public CReportWrap() 
        {
            _ReportDetail = new ReportDetail();
            
        }
        private ReportDetail _ReportDetail;
        
        public ReportDetail ReportDetail 
        { 
            get { return _ReportDetail; }
            set { this._ReportDetail = value; }
        }
        [DisplayName("健檢報告詳細ID")]
        [Display(Name = "健檢報告詳細ID")]
        public int ReportDetailId
        {
            get { return _ReportDetail.ReportDetailId; }
            set { this.ReportDetail.ReportDetailId = value; }
        }
        [DisplayName("健檢報告ID")]
        [Display(Name = "健檢報告ID")]
        public int? ReportId 
        {
            get { return _ReportDetail.ReportId; } 
            set { this.ReportDetail.ReportId = value; } 
        }
        [DisplayName("項目ID")]
        public int? ItemId 
        { 
            get { return _ReportDetail.ItemId; }
            set { this.ReportDetail.ItemId = value; }
        }
        [DisplayName("健檢結果")]
        public string Result
        {
            get { return _ReportDetail.Result; }
            set { this.ReportDetail.Result = value; }
        }

        //public virtual Item Item
        //{
        //    get { return _ReportDetail.Item; }
        //    set { this.ReportDetail.Item = value; }
        //}

        public virtual HealthReport Report 
        {
            get { return _ReportDetail.Report; }
            set { this.ReportDetail.Report = value; }
        }

      


    }
}
