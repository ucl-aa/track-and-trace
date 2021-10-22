using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders(string tracingId = "");
    }
}