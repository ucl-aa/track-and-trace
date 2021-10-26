using System.Collections.Generic;
using backend.Models;

namespace backend.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders(string tracingId = "");
    }
}