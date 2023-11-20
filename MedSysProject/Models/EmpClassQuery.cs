namespace MedSysProject.Models
{
    public class EmpClassQuery
    {
        private MedSysContext _db;

        public EmpClassQuery(MedSysContext db)
        {
            _db = db;
        }

        public string GetEmployeeClassNameById(int employeeClassId)
        {

            // 在此處使用你的資料存取邏輯，從資料庫中取得對應的名稱
            var q = (from c in _db.EmployeeClasses
                    where c.EmployeeClassId == employeeClassId
                    select c.Class).ToList();


            // 確認是否找到相應的 EmployeeClass
            if (q != null)
            {
                return q[0].ToString();
            }

            return "";
        }
    }
}
