using System.Collections.Generic;
using Backend.Models;

namespace Backend.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders(string tracingId = "");

        Order AddOrder(Order order);
    }
}