using System.Collections.Generic;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistency
{
    public class OrderRepository : DbContext, IOrderRepository
    {
        private DbSet<Order> _orders;
        public IEnumerable<Order> Orders => _orders;

        public OrderRepository(DbContextOptions<OrderRepository> options) : base(options)
        {
        }
    }
}