using System.Collections.Generic;
using Backend.Controllers;
using Backend.Models;
using Backend.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Test.Controllers
{
    public class DropOffControllerTest
    {
        private readonly DropOffController _controller;
        private readonly IDropOffService _dropOffService;

        public DropOffControllerTest()
        {
            _dropOffService = A.Fake<IDropOffService>();
            A.CallTo(() => _dropOffService.GetAsync(0))
                .Returns(new List<DropOff>());
            _controller = new DropOffController(_dropOffService);
        }

        [Fact]
        public void Should_beAssignableToControllerBase()
        {
            _controller.Should().BeAssignableTo<ControllerBase>();
        }

        [Fact]
        public void Should_useDropOffService_when_GetIsCalled()
        {
            _controller.Get(null);

            A.CallTo(() => _dropOffService.GetAsync(null))
                .MustHaveHappenedOnceExactly();
        }
    }
}