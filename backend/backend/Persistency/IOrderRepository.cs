using System.Collections.Generic;
using Backend.Models;

namespace Backend.Persistency
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; }
    }
}