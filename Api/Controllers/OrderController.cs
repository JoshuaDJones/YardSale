using Core.YardSale.Contracts;
using Core.YardSale.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("GetUserOrders")]
        public IActionResult GetUserOrders([FromBody] OrderData orderData)
        {
            var result = _orderRepository.GetUserOrders(orderData);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateOrder")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            var result = _orderRepository.CreateOrder(order);
            return Ok(result);
        }

        [HttpPost]
        [Route("ChangeOrderStatus")]
        public IActionResult ChangeOrderStatus([FromBody] MarkOrderData orderData)
        {
            var result = _orderRepository.ChangeOrderStatus(orderData);
            return Ok(result);
        }
    }
}
