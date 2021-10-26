using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using Backend.Persistency;

namespace Backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IEnumerable<Order> GetOrders(string tracingId = "")
        {
            return _orderRepository.Orders.Where(o => o.TracingId == tracingId);
        }
    }
}