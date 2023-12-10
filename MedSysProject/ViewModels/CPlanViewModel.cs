using MedSysProject.Models;
using System.ComponentModel;

namespace MedSysProject.ViewModels
{
    public class CPlanViewModel
    {
        private Plan _plan;
        private PlanRef _planRef;
        private Item _item;
        public CPlanViewModel() {
            _plan = new Plan();
           _planRef = new PlanRef();
            _item = new Item();
        }
        //public Plan plan { get { return _plan; } set { this._plan = value; } }
        //public PlanRef planRef { get { return _planRef; } set { this._planRef = value; } }
        //public Item item { get { return _item; } set { this._item = value; } }
        public int PlanId { get; set; }

        [DisplayName("方案名稱")]
        public string PlanName { get; set; }

        [DisplayName("方案描述")]
        public string PlanDescription { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("類別名稱")]
        public string ProjectName { get; set; }
        [DisplayName("類別價格")]
        public double? ProjectPrice { get; set; }

        public int ItemId { get; set; }

        [DisplayName("細項名稱")]
        public string ItemName { get; set; }


    }
}
