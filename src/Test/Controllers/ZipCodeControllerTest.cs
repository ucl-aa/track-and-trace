using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.DataTransferObjects;
using Backend.Exceptions;
using Backend.Loggers;
using Backend.Models;
using Backend.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Test.Controllers
{
    public class ZipCodeControllerTest
    {
        private readonly IZipCodeService _zipCodeService;
        private readonly ZipCodeController _controller;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly ILogger<ZipCodeController> _logger;

        public ZipCodeControllerTest()
        {
            List<ZipCode> zipCodes = new List<ZipCode>
            {
                new ()
                {
                    ZipCodeId = 1,
                    City = "Aalborg",
                    ZipCodeValue = "4562",
                },
                new ()
                {
                    ZipCodeId = 2,
                    City = "Skanderborg",
                    ZipCodeValue = "3216",
                },
            };

            _exceptionLogger = A.Fake<IExceptionLogger>();
            _zipCodeService = A.Fake<IZipCodeService>();
            _logger = new LoggerFactory().CreateLogger<ZipCodeController>();
            A.CallTo(() => _zipCodeService.GetAsync(null)).Returns(zipCodes);
            _controller = new ZipCodeController(
                _zipCodeService,
                _exceptionLogger,
                _logger);
        }

        [Fact]
        private void Should_beControllerBase()
        {
            _controller.Should().BeAssignableTo(typeof(ControllerBase));
        }

        public class GetTest : ZipCodeControllerTest
        {
            [Fact]
            public void Should_callService_when_gettingZipCodes()
            {
                _controller.Get(null);

                A.CallTo(() => _zipCodeService.GetAsync(null)).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnNotFoundAction_when_serviceThrowsNotFoundException()
            {
                int id = 654;
                EntityNotFoundException entityNotFoundException = new EntityNotFoundException(nameof(ZipCode), id);
                A.CallTo(() => _zipCodeService.GetAsync(null))
                    .Throws(entityNotFoundException);

                ActionResult<IEnumerable<ZipCode>> returnAction = await _controller.Get(null);

                returnAction.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            }

            [Fact]
            public async void Should_alwaysLogException_when_serviceThrowsExceptionOnGet()
            {
                Exception exception = new Exception();
                A.CallTo(() => _zipCodeService.GetAsync(null)).Throws(exception);

                Func<Task<ActionResult<IEnumerable<ZipCode>>>> action = _controller.
                    Awaiting(x => x.Get(null));
                await action.Should().ThrowAsync<Exception>();

                A.CallTo(() => _exceptionLogger.Log(exception, nameof(ZipCodeController), _logger))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class PostTest : ZipCodeControllerTest
        {
            [Fact]
            public async void Should_returnCreatedAtAction_when_postingZipCode()
            {
                ZipCodeDto zipCodeDto = new ZipCodeDto();

                ActionResult<ZipCode> result = await _controller.Post(zipCodeDto);

                result.Result.Should().BeOfType(typeof(CreatedAtActionResult));
            }

            [Fact]
            public async void Should_useServiceWhen_when_postingZipCode()
            {
                ZipCodeDto zipCodeDto = new ZipCodeDto
                {
                    City = "Tommerup",
                    ZipCodeValue = "5690",
                };
                A.CallTo(() => _zipCodeService.AddAsync(zipCodeDto)).Returns(new ZipCode
                {
                    City = zipCodeDto.City,
                    ZipCodeValue = zipCodeDto.ZipCodeValue,
                });

                ActionResult<ZipCode> returnCode = await _controller.Post(zipCodeDto);

                A.CallTo(() => _zipCodeService.AddAsync(zipCodeDto))
                    .MustHaveHappenedOnceExactly();
                var result = returnCode.Result as CreatedAtActionResult;
                ((ZipCode)result?.Value)?.City.Should().Be(zipCodeDto.City);
                ((ZipCode)result?.Value)?.ZipCodeValue.Should().Be(zipCodeDto.ZipCodeValue);
            }

            [Fact]
            public async void Should_log_when_exceptionIsThrowDuringPost()
            {
                Exception exception = new Exception();
                A.CallTo(() => _zipCodeService.AddAsync(null))
                    .Throws(exception);
                Func<Task<ActionResult<ZipCode>>> action = _controller
                    .Awaiting(x => x.Post(null));
                await action.Should().ThrowAsync<Exception>();

                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(ZipCodeController), _logger))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class DeleteTest : ZipCodeControllerTest
        {
            [Fact]
            public void Should_useService_when_deleting()
            {
                int id = 645;

                _controller.Delete(id);

                A.CallTo(() => _zipCodeService.DeleteAsync(id))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnNotFound_when_serviceThrowsNotFoundException()
            {
                int id = 231;
                EntityNotFoundException exception = new EntityNotFoundException(nameof(ZipCode), id);
                A.CallTo(() => _zipCodeService.DeleteAsync(id)).Throws(exception);

                var result = await _controller.Delete(id);

                result.Should().BeOfType(typeof(NotFoundObjectResult));
            }

            [Fact]
            public async void Should_log_when_serviceThrowsExceptionOnDelete()
            {
                int id = 98;
                Exception exception = new Exception();
                A.CallTo(() => _zipCodeService.DeleteAsync(id)).Throws(exception);

                Func<Task<ActionResult>> action = _controller.Awaiting(x => x.Delete(id));
                await action.Should().ThrowAsync<Exception>();
                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(ZipCodeController), _logger))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class PutTest : ZipCodeControllerTest
        {
            [Fact]
            public void Should_callService_when_updatingZipCode()
            {
                int id = 564;
                ZipCodeDto zipCodeDto = new ZipCodeDto();

                _controller.Put(id, zipCodeDto);

                A.CallTo(() => _zipCodeService.UpdateAsync(id, zipCodeDto))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_log_when_serviceThrowsExceptionOnPut()
            {
                int id = 5;
                ZipCodeDto zipCodeDto = new ZipCodeDto();
                Exception exception = new Exception();
                A.CallTo(() => _zipCodeService.UpdateAsync(id, zipCodeDto))
                    .Throws(exception);

                Func<Task<ActionResult<ZipCode>>> action =
                    _controller.Awaiting(x => x.Put(id, zipCodeDto));

                await action.Should().ThrowAsync<Exception>();
                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(ZipCodeController), _logger))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnCreated_when_putCreatesNewEntity()
            {
                int id = 9135319;
                ZipCodeDto zipCodeDto = new ZipCodeDto
                {
                    City = "Vordingborg",
                    ZipCodeValue = "8620",
                };
                ZipCode zipCode = new ZipCode
                {
                    ZipCodeId = id,
                    City = zipCodeDto.City,
                    ZipCodeValue = zipCodeDto.ZipCodeValue,
                };
                EntityNotFoundException exception = new EntityNotFoundException(nameof(ZipCode), id);
                A.CallTo(() => _zipCodeService.GetAsync(id)).Throws(exception);
                A.CallTo(() => _zipCodeService.UpdateAsync(id, zipCodeDto)).Returns(zipCode);

                ActionResult<ZipCode> actionResult = await _controller.Put(id, zipCodeDto);

                actionResult.Result.Should().BeOfType(typeof(CreatedAtActionResult));
                var result = actionResult.Result as CreatedAtActionResult;
                ((ZipCode)result?.Value)?.City.Should().Be(zipCodeDto.City);
                ((ZipCode)result?.Value)?.ZipCodeValue.Should().Be(zipCodeDto.ZipCodeValue);
            }

            [Fact]
            public async void Should_returnOk_when_putOverwritesExistingEntity()
            {
                int id = 4;
                ZipCodeDto zipCodeDto = new ZipCodeDto
                {
                    City = "Vordingborg",
                    ZipCodeValue = "8620",
                };
                ZipCode zipCode = new ZipCode
                {
                    ZipCodeId = id,
                    City = zipCodeDto.City,
                    ZipCodeValue = zipCodeDto.ZipCodeValue,
                };
                ZipCode oldZipCode = new ZipCode
                {
                    ZipCodeId = id,
                };
                A.CallTo(() => _zipCodeService.GetAsync(id))
                    .Returns(new List<ZipCode>
                    {
                        oldZipCode,
                    });
                A.CallTo(() => _zipCodeService.UpdateAsync(id, zipCodeDto)).Returns(zipCode);

                ActionResult<ZipCode> actionResult = await _controller.Put(id, zipCodeDto);

                actionResult.Result.Should().BeOfType(typeof(OkObjectResult));
                var result = actionResult.Result as CreatedAtActionResult;
                ((ZipCode)result?.Value)?.City.Should().Be(zipCodeDto.City);
                ((ZipCode)result?.Value)?.ZipCodeValue.Should().Be(zipCodeDto.ZipCodeValue);
            }
        }
    }
}