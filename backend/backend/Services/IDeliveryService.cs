using System.Collections.Generic;
using Backend.Models;

namespace Backend.Services
{
    public interface IDeliveryService
    {
        IEnumerable<Delivery> GetOrders(string tracingId);

        Delivery AddOrder(Delivery delivery);
    }
}