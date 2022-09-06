using Core.YardSale.Contracts;
using Core.YardSale.Orders;
using Core.YardSale.Products;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.YardSale
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDatabaseRepository _databaseRepository;

        public OrderRepository(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public int ChangeOrderStatus(MarkOrderData markOrderData)
        {
            int statusId = 0;

            if (markOrderData.StatusId == 1)
            {
                statusId = 2;
            }
            else if (markOrderData.StatusId == 2)
            {
                statusId = 4;
            }

            int retVal = _databaseRepository.GetRetVal("usp_Order_Change_Status", new List<object>() { markOrderData.OrderId, statusId });
            return retVal;
        }

        public int CreateOrder(Order order)
        {
            int retVal = _databaseRepository.GetRetVal("usp_Order_Create", new List<object>() { order.Product.ProductId, order.LkOrderStatus.LkOrderStatusId, order.OrderDateTime, order.OrderCustomerAddress, order.OrderTotal });
            return retVal;
        }

        public List<Order> GetUserOrders(OrderData orderData)
        {
            List<Order> orders = new();
            DataTable dt = _databaseRepository.GetDT("usp_Order_Get_User_List", new List<object> { orderData.UserId, orderData.StatusId });

            foreach (DataRow row in dt.Rows)
            {
                Category category = new()
                {
                    category_id = Convert.ToInt32(row["category_id"]),
                    category_desc = row["category_desc"].ToString() ?? ""
                };

                Product product = new()
                {
                    ProductId = Convert.ToInt32(row["product_id"]),
                    UserId = Convert.ToInt32(row["product_id"]),
                    Category = category,
                    ProductTitle = row["product_title"].ToString() ?? "",
                    ProductDescription = row["product_description"].ToString() ?? "",
                    ProductCost = Convert.ToDecimal(row["product_cost"]),
                    ProductThumbnailPhotoUrl = row["product_thumbnail_photo_url"].ToString() ?? "",
                    ProductThumbnailPhotoFilename = row["product_thumbnail_photo_filename"].ToString() ?? "",
                    ProductIsActive = Convert.ToBoolean(row["product_is_active"]),
                    ProductIsSold = Convert.ToBoolean(row["product_is_sold"]),

                };

                LkOrderStatus status = new()
                {
                    LkOrderStatusId = Convert.ToInt32(row["lk_order_status_id"]),
                    LkOrderStatusDesc = row["lk_order_status_desc"].ToString() ?? ""
                };

                Order order = new()
                {
                    OrderId = Convert.ToInt32(row["order_id"]),
                    Product = product,
                    LkOrderStatus = status,
                    OrderDateTime = Convert.ToDateTime(row["order_date_time"]),
                    OrderCustomerAddress = row["order_customer_address"].ToString() ?? "",
                    OrderTotal = Convert.ToDecimal(row["order_total"])
                };

                orders.Add(order);
            }
            return orders;
        }
    }
}
