using Core.YardSale.Orders;

namespace YardSale.ViewModels
{
    public class OrdersViewModel
    {
        public List<Order> Orders { get; set; } = new List<Order>();
        public int LkOrderStatusId { get; set; }
    }
}
