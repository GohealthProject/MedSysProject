namespace MedSysProject.Models
{
    public class CCartItem
    {
        public Product Product { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int 小計 { get; set; }
        public int count { get; set; }

    }
}
