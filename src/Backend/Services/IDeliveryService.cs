using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IDeliveryService
    {
        Task<IEnumerable<Delivery>> GetAsync(int? id);

        IEnumerable<Delivery> GetOrders(string tracingId);

        Delivery AddOrder(Delivery delivery);
    }
}