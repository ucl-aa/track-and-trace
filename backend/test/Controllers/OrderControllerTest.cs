using System.Collections.Generic;
using Backend.Controllers;
using Backend.Models;
using Backend.Services;
using FakeItEasy;
using Xunit;

namespace Test.Controllers
{
    public class OrderControllerTest
    {
        private readonly IOrderService _orderService;
        private readonly OrderController _controller;

        public OrderControllerTest()
        {
            List<Order> orders = new List<Order>
            {
                new ()
                {
                    Id = 1,
                    TracingId = "random ass tracing id",
                },
                new ()
                {
                    Id = 15,
                    TracingId = "Other Tracing Id",
                },
            };
            _orderService = A.Fake<IOrderService>();
            A.CallTo(() => _orderService.GetOrders("")).Returns(orders);
            _controller = new OrderController(_orderService);
        }

        [Fact]
        public void Should_getOrdersFromRepository_when_gettingOrders()
        {
            _controller.Get();
            A.CallTo(() => _orderService.GetOrders(""))
                .MustHaveHappenedOnceExactly();
        }
    }
}