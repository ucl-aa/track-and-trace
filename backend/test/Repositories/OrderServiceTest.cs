using System.Collections.Generic;
using backend.Models;
using backend.Persistency;
using backend.Services;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace test.Repositories
{
    public class OrderServiceTest
    {
        private readonly OrderService _orderService;
        private readonly IOrderRepository _orderRepository;
        private readonly List<Order> _orders;
        public OrderServiceTest()
        {
            _orderRepository = A.Fake<IOrderRepository>();
            _orders = new List<Order>();
            A.CallTo(() => _orderRepository.Orders).Returns(_orders);

            _orderService = new OrderService(_orderRepository);
        }
        
        [Fact]
        public void Should_implementInterface()
        {
            _orderService.Should().BeAssignableTo(typeof(IOrderService));
        }

        [Fact]
        public void Should_callDatabase_when_gettingOrders()
        {
            _orderService.GetOrders();
            
            A.CallTo(() => _orderRepository.Orders).MustHaveHappened();
        }

        [Fact]
        public void Should_sortOrders_when_optionalParameterIsGiven()
        {
            List<Order> orders = new List<Order>
            {
                new()
                {
                    Id = 1,
                    TracingId = "first id"
                },
                new()
                {
                    Id = 14,
                    TracingId = "different tracing id"
                },
                new()
                {
                    Id = 5,
                    TracingId = "#nemt"
                }
            };
            _orders.AddRange(orders);
            List<Order> singleOrder = new(_orderService.GetOrders("first id"));
            List<Order> empty = new(_orderService.GetOrders("non existing id"));

            singleOrder.Count.Should().Be(1);
            empty.Count.Should().Be(0);
        }

        [Fact]
        public void Should_notSortOrders_when_optionalParameterIsEmpty()
        {
            List<Order> orders = new List<Order>
            {
                new()
                {
                    Id = 15,
                    TracingId = "Random ass id"
                },
                new()
                {
                    Id = 2,
                    TracingId = "John"
                },
                new()
                {
                    Id = 754,
                    TracingId = "Molly"
                }
            };
            _orders.AddRange(orders);

            List<Order> results = new(_orderRepository.Orders);

            results.Count.Should().Be(3);
        }
    }
}