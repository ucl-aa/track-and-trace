using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Persistency
{
    public class OrderRepository : DbContext, IOrderRepository
    {
        public OrderRepository(DbContextOptions<OrderRepository> options)
            : base(options)
        {
        }

        private DbSet<Order> _orders;

        public IEnumerable<Order> Orders => _orders;
    }
}