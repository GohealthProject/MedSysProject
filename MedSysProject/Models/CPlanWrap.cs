using System.ComponentModel;

namespace MedSysProject.Models
{
    public class CPlanWrap
    {
        private  Plan _plan;
        private Project _project;
        private Item _item;
        public CPlanWrap() { 
            _plan=new Plan();
            _project=new Project();
            _item=new Item();
        }
        public Plan plan { get { return _plan; }set { this._plan = value; } }
        public Project project { get { return _project; } set { this._project = value; } }
        public Item item { get { return _item; } set { this._item = value; } }

        public int PlanId { get { return _plan.PlanId; } set { this._plan.PlanId = value; } }

        [DisplayName("方案名稱")]
        public string PlanName { get { return _plan.PlanName; } set { this._plan.PlanName = value; } }

        [DisplayName("方案描述")]
        public string PlanDescription { get { return _plan.PlanDescription; } set { this._plan.PlanDescription = value; } }
        public int ProjectId { get { return _project.ProjectId; } set { this._project.ProjectId = value; } }

        [DisplayName("類別名稱")]
        public string ProjectName { get { return _project.ProjectName; } set { this._project.ProjectName = value; } }
        [DisplayName("類別價格")]
        public double? ProjectPrice { get { return _project.ProjectPrice; } set { this._project.ProjectPrice = value; } }
       
        public int ItemId { get { return _item.ItemId; } set { this._item.ItemId = value; } }

 [DisplayName("細項名稱")]
        public string ItemName { get { return _item.ItemName; } set { this._item.ItemName = value; } }
    }

}
