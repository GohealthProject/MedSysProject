using System.ComponentModel;

namespace MedSysProject.Models
{
    public class CCartItem
    {
        public Product Product { get; set; }
        [DisplayName("產品名稱")]
        public string ProductName { get; set; }
        [DisplayName("單價")]
        public int UnitPrice { get; set; }
        [DisplayName("產品總計")]
        public int 小計 { get; set; }
        [DisplayName("數量")]
        public int count { get; set; }

    }
}
