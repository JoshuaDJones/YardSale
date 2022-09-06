using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Orders
{
    public class LkOrderStatus
    {
        public int LkOrderStatusId { get; set; }
        public string LkOrderStatusDesc { get; set; } = string.Empty;
    }

    public enum OrderStatusType
    {
        Pending = 1,
        Shipped = 2,
        Cancelled = 3,
        Complete = 4,
    }
}
