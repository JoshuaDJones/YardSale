using Core.YardSale.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Orders
{
    public class Order
    {
        public int OrderId { get; set; }
        public Product Product { get; set; } = new Product();
        public LkOrderStatus LkOrderStatus { get; set; } = new LkOrderStatus();
        public DateTime OrderDateTime { get; set; }
        public string OrderCustomerAddress { get; set; } = string.Empty;
        public Decimal OrderTotal { get; set; }
    }
}
