using System.Collections.Generic;
using Backend.Controllers;
using Backend.Exceptions;
using Backend.Loggers;
using Backend.Models;
using Backend.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Test.Controllers
{
    public class ZipCodeControllerTest
    {
        private readonly IZipCodeService _zipCodeService;
        private readonly ZipCodeController _controller;
        private readonly IExceptionLogger _exceptionLogger;

        public ZipCodeControllerTest()
        {
            List<ZipCode> zipCodes = new List<ZipCode>
            {
                new ()
                {
                    City = "Aalborg",
                    ZipCodeValue = "4562",
                },
                new ()
                {
                    City = "Skanderborg",
                    ZipCodeValue = "3216",
                },
            };

            _exceptionLogger = A.Fake<IExceptionLogger>();
            _zipCodeService = A.Fake<IZipCodeService>();
            A.CallTo(() => _zipCodeService.GetAsync(null)).Returns(zipCodes);
            _controller = new ZipCodeController(_zipCodeService, _exceptionLogger);
        }

        [Fact]
        public void Should_beControllerBase()
        {
            _controller.Should().BeAssignableTo(typeof(ControllerBase));
        }

        [Fact]
        public void Should_callService_when_gettingZipCodes()
        {
            _controller.Get(null);

            A.CallTo(() => _zipCodeService.GetAsync(null)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void Should_returnNotFoundActionAndLog_when_serviceThrowsNotFoundException()
        {
            int id = 654;
            EntityNotFoundException entityNotFoundException = new EntityNotFoundException(nameof(ZipCode), id);
            A.CallTo(() => _zipCodeService.GetAsync(null))
                .Throws(entityNotFoundException);

            ActionResult<IEnumerable<ZipCode>> returnAction = await _controller.Get(null);

            returnAction.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            A.CallTo(() => _exceptionLogger
                .LogException(entityNotFoundException, nameof(_controller), null))
                .MustHaveHappenedOnceExactly();
        }
    }
}