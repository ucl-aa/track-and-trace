#nullable enable
using System.Collections.Generic;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IEnumerable<Order> Get(string tracingId = "")
        {
            return _orderService.GetOrders(tracingId);
        }

        public Order Add(Order order)
        {
            return _orderService.AddOrder(order);
        }
    }
}