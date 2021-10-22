using System.Collections.Generic;
using System.Linq;
using backend.Models;
using backend.Persistency;

namespace backend.Repositories
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