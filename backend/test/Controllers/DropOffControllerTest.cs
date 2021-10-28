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
    public class DropOffControllerTest
    {
        private readonly IDropOffService _dropOffService;
        private readonly DropOffController _controller;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly ILogger _logger;

        public DropOffControllerTest()
        {
            List<DropOff> zipCodes = new List<DropOff>
            {
                new ()
                {
                    Key = 1,
                    Name = "Lunds Boller",
                    Street = "Gadevej 42",
                    ZipCode = new ()
                    {
                        ZipCodeId = 1,
                        City = "Aalborg",
                        ZipCodeValue = "4562",
                    }
                },
                new ()
                {
                    Key = 1,
                    Name = "Bodils Blomster",
                    Street = "Blomstergården 1F",
                    ZipCode = new ()
                    {
                        ZipCodeId = 2,
                        City = "Odense",
                        ZipCodeValue = "5000",
                    }
                },
            };

            _exceptionLogger = A.Fake<IExceptionLogger>();
            _dropOffService = A.Fake<IDropOffService>();
            _logger = new LoggerFactory().CreateLogger("test logger");
            A.CallTo(() => _dropOffService.GetAsync(null)).Returns(zipCodes);
            _controller = new DropOffController(
                _dropOffService,
                _exceptionLogger,
                _logger);
        }

        [Fact]
        private void Should_beControllerBase()
        {
            _controller.Should().BeAssignableTo(typeof(ControllerBase));
        }

        public class GetTest : DropOffControllerTest
        {
            [Fact]
            public void Should_callService_when_gettingDropOffs()
            {
                _controller.Get(null);

                A.CallTo(() => _dropOffService.GetAsync(null)).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnNotFoundAction_when_serviceThrowsNotFoundException()
            {
                int id = 654;
                EntityNotFoundException entityNotFoundException = new EntityNotFoundException(nameof(DropOff), id);
                A.CallTo(() => _dropOffService.GetAsync(null))
                    .Throws(entityNotFoundException);

                ActionResult<IEnumerable<DropOff>> returnAction = await _controller.Get(null);

                returnAction.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            }

            [Fact]
            public async void Should_alwaysLogException_when_serviceThrowsExceptionOnGet()
            {
                Exception exception = new Exception();
                A.CallTo(() => _dropOffService.GetAsync(null)).Throws(exception);

                Func<Task<ActionResult<IEnumerable<DropOff>>>> action = _controller.
                    Awaiting(x => x.Get(null));
                await action.Should().ThrowAsync<Exception>();

                A.CallTo(() => _exceptionLogger.Log(exception, nameof(DropOffController), _logger))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class PostTest : DropOffControllerTest
        {
            [Fact]
            public async void Should_returnCreatedAtAction_when_postingDropOff()
            {
                DropOffDto zipCodeDto = new DropOffDto();

                ActionResult<DropOff> result = await _controller.Post(zipCodeDto);

                result.Result.Should().BeOfType(typeof(CreatedAtActionResult));
            }

            [Fact]
            public async void Should_useServiceWhen_when_postingDropOff()
            {
                DropOffDto dropOffDto = new DropOffDto
                {
                    Name = "Toms Rum",
                    Street = "Baggyde 6",
                    ZipCode = new ()
                    {
                        ZipCodeId = 1,
                        City = "Holstebro",
                        ZipCodeValue = "7500",
                    }
                };
                A.CallTo(() => _dropOffService.AddAsync(dropOffDto)).Returns(new DropOff
                {
                    Name = dropOffDto.Name,
                    Street = dropOffDto.Street,
                    ZipCode = dropOffDto.ZipCode,
                });

                ActionResult<DropOff> returnCode = await _controller.Post(dropOffDto);

                A.CallTo(() => _dropOffService.AddAsync(dropOffDto))
                    .MustHaveHappenedOnceExactly();
                var result = returnCode.Result as CreatedAtActionResult;
                ((DropOff)result?.Value)?.Name.Should().Be(dropOffDto.Name);
                ((DropOff)result?.Value)?.Street.Should().Be(dropOffDto.Street);
                ((DropOff)result?.Value)?.ZipCode.Should().Be(dropOffDto.ZipCode);
            }

            [Fact]
            public async void Should_log_when_exceptionIsThrowDuringPost()
            {
                Exception exception = new Exception();
                A.CallTo(() => _dropOffService.AddAsync(null))
                    .Throws(exception);
                Func<Task<ActionResult<DropOff>>> action = _controller
                    .Awaiting(x => x.Post(null));
                await action.Should().ThrowAsync<Exception>();

                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(DropOffController), _logger))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class DeleteTest : DropOffControllerTest
        {
            [Fact]
            public void Should_useService_when_deleting()
            {
                int id = 645;

                _controller.Delete(id);

                A.CallTo(() => _dropOffService.DeleteAsync(id))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnNotFound_when_serviceThrowsNotFoundException()
            {
                int id = 231;
                EntityNotFoundException exception = new EntityNotFoundException(nameof(DropOff), id);
                A.CallTo(() => _dropOffService.DeleteAsync(id)).Throws(exception);

                var result = await _controller.Delete(id);

                result.Should().BeOfType(typeof(NotFoundObjectResult));
            }

            [Fact]
            public async void Should_log_when_serviceThrowsExceptionOnDelete()
            {
                int id = 98;
                Exception exception = new Exception();
                A.CallTo(() => _dropOffService.DeleteAsync(id)).Throws(exception);

                Func<Task<ActionResult>> action = _controller.Awaiting(x => x.Delete(id));
                await action.Should().ThrowAsync<Exception>();
                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(DropOffController), _logger))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class PutTest : DropOffControllerTest
        {
            [Fact]
            public void Should_callService_when_updatingDropOff()
            {
                int id = 564;
                DropOffDto zipCodeDto = new DropOffDto();

                _controller.Put(id, zipCodeDto);

                A.CallTo(() => _dropOffService.UpdateAsync(id, zipCodeDto))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_log_when_serviceThrowsExceptionOnPut()
            {
                int id = 5;
                DropOffDto zipCodeDto = new DropOffDto();
                Exception exception = new Exception();
                A.CallTo(() => _dropOffService.UpdateAsync(id, zipCodeDto))
                    .Throws(exception);

                Func<Task<ActionResult<DropOff>>> action =
                    _controller.Awaiting(x => x.Put(id, zipCodeDto));

                await action.Should().ThrowAsync<Exception>();
                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(DropOffController), _logger))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnCreated_when_putCreatesNewEntity()
            {
                int id = 8213172;
                DropOffDto dropOffDto = new DropOffDto
                {
                    Name = "Kims Stue",
                    Street = "Låkortsgade 6",
                    ZipCode = new()
                    {
                        ZipCodeId = 1,
                        City = "København",
                        ZipCodeValue = "5911",
                    }
                };
                DropOff dropOff = new DropOff
                {
                    Key = id,
                    Name = dropOffDto.Name,
                    Street = dropOffDto.Street,
                    ZipCode = dropOffDto.ZipCode,
                };
                EntityNotFoundException exception = new EntityNotFoundException(nameof(DropOff), id);
                A.CallTo(() => _dropOffService.GetAsync(id)).Throws(exception);
                A.CallTo(() => _dropOffService.UpdateAsync(id, dropOffDto)).Returns(dropOff);

                ActionResult<DropOff> actionResult = await _controller.Put(id, dropOffDto);

                actionResult.Result.Should().BeOfType(typeof(CreatedAtActionResult));
                var result = actionResult.Result as CreatedAtActionResult;
                ((DropOff)result?.Value)?.Name.Should().Be(dropOffDto.Name);
                ((DropOff)result?.Value)?.Street.Should().Be(dropOffDto.Street);
                ((DropOff)result?.Value)?.ZipCode.Should().Be(dropOffDto.ZipCode);
            }

            [Fact]
            public async void Should_returnOk_when_putOverwritesExistingEntity()
            {
                int id = 8213172;
                DropOffDto dropOffDto = new DropOffDto
                {
                    Name = "Kurts Knapper",
                    Street = "Tøjvej 99",
                    ZipCode = new()
                    {
                        ZipCodeId = 1,
                        City = "Odense N",
                        ZipCodeValue = "6500",
                    }
                };
                DropOff dropOff = new DropOff
                {
                    Key = id,
                    Name = "Sigurds Bjørne",
                    Street = "Brændsti 123",
                    ZipCode = new()
                    {
                        ZipCodeId = 2,
                        City = "Odense",
                        ZipCodeValue = "5000",
                    }
                };
                DropOff oldDropOff = new DropOff
                {
                    Key = id,
                };
                A.CallTo(() => _dropOffService.GetAsync(id))
                    .Returns(new List<DropOff>
                    {
                        oldDropOff,
                    });
                A.CallTo(() => _dropOffService.UpdateAsync(id, dropOffDto)).Returns(dropOff);

                ActionResult<DropOff> actionResult = await _controller.Put(id, dropOffDto);

                actionResult.Result.Should().BeOfType(typeof(OkObjectResult));
                var result = actionResult.Result as CreatedAtActionResult;
                ((DropOff)result?.Value)?.Name.Should().Be(dropOffDto.Name);
                ((DropOff)result?.Value)?.Street.Should().Be(dropOffDto.Street);
                ((DropOff)result?.Value)?.ZipCode.Should().Be(dropOffDto.ZipCode);
            }
        }
    }
}