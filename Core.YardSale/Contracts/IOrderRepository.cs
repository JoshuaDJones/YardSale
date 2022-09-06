using Core.YardSale.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Contracts
{
    public interface IOrderRepository
    {
        int CreateOrder(Order order);
        List<Order> GetUserOrders(OrderData orderData);
        int ChangeOrderStatus(MarkOrderData markOrderData);
    }
}
