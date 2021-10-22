using System.Collections.Generic;
using backend.Models;

namespace backend.Persistency
{
    public interface IOrderRepository
    { 
        IEnumerable<Order> Orders { get; }
    }
}