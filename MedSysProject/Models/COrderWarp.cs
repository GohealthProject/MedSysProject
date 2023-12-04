using System.ComponentModel;

namespace MedSysProject.Models
{
    public class COrderWarp
    {
        private Order _order = null;
        public COrderWarp()
        {
            _order = new Order(); 
        }
        public Order order { get { return _order; }set { _order = value; } }
        public int OrderId { get {return this._order.OrderId; } set { this._order.OrderId = value; } }
        [DisplayName("訂單日期")]
        public DateTime OrderDate { get { return this._order.OrderDate; } set { this._order.OrderDate = value; } }
        [DisplayName("客戶號碼")]
        public int? MemberId { get { return this._order.MemberId; } set { this._order.MemberId = value; } }
        [DisplayName("運送方式")]
        public int? ShipId { get { return this._order.ShipId; } set { this._order.ShipId = value; } }
        [DisplayName("支付方式")]
        public int? PayId { get { return this._order.PayId; } set { this._order.PayId = value; } }
        [DisplayName("訂單狀態")]
        public int? StateId { get { return this._order.StateId; } set { this._order.StateId = value; } }
        [DisplayName("寄出日期")]
        public DateTime? ShipDate { get { return this._order.ShipDate; } set { this._order.ShipDate = value; } }
        [DisplayName("預計到達日期")]
        public DateTime? DeliveryDate { get { return this._order.DeliveryDate; } set { this._order.DeliveryDate = value; } }

        public virtual Member Member { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual OrderPay Pay { get; set; }

        public virtual OrderShip Ship { get; set; }

        public virtual OrderState State { get; set; }
    }
}
