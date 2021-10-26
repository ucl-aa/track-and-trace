#nullable enable
using System.Collections.Generic;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryController : Controller
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public IEnumerable<Delivery> Get(string tracingId = "")
        {
            return _deliveryService.GetOrders(tracingId);
        }

        [HttpPost]
        public Delivery Post(Delivery delivery)
        {
            return _deliveryService.AddOrder(delivery);
        }
    }
}