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
    public class StatusControllerTest
    {
        private readonly IStatusService _statusService;
        private readonly IDeliveryService _deliveryService;
        private readonly StatusController _controller;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly ILogger _logger;
        private readonly Delivery _delivery;
        private readonly int _deliveryId;

        public StatusControllerTest()
        {
            List<Status> statuses = new List<Status>
            {
                new ()
                {
                    Id = 1,
                    UpdateTime = DateTime.Now,
                    Message = "Delivered",
                },
                new ()
                {
                    Id = 2,
                    UpdateTime = DateTime.Today.AddDays(1),
                    Message = "Ordered",
                },
            };

            _delivery = new Delivery();
            _deliveryId = 546;

            List<Delivery> deliveries = new List<Delivery>
            {
                _delivery,
            };

            _exceptionLogger = A.Fake<IExceptionLogger>();
            _statusService = A.Fake<IStatusService>();
            _deliveryService = A.Fake<IDeliveryService>();
            _logger = new LoggerFactory().CreateLogger("test logger");
            A.CallTo(() => _statusService.GetAsync(null)).Returns(statuses);
            A.CallTo(() => _deliveryService.GetAsync(_deliveryId)).Returns(deliveries);
            _controller = new StatusController(
                _statusService,
                _deliveryService,
                _exceptionLogger,
                _logger);
        }

        [Fact]
        private void Should_beControllerBase()
        {
            _controller.Should().BeAssignableTo(typeof(ControllerBase));
        }

        public class GetTest : StatusControllerTest
        {
            [Fact]
            public void Should_callService_when_gettingStatuses()
            {
                _controller.Get(null);

                A.CallTo(() => _statusService.GetAsync(null)).MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnNotFoundAction_when_serviceThrowsNotFoundException()
            {
                int id = 456;
                EntityNotFoundException entityNotFoundException = new EntityNotFoundException(nameof(Status), id);
                A.CallTo(() => _statusService.GetAsync(null))
                    .Throws(entityNotFoundException);

                ActionResult<IEnumerable<Status>> returnAction = await _controller.Get(null);

                returnAction.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            }

            [Fact]
            public async void Should_alwaysLogException_when_serviceThrowsExceptionOnGet()
            {
                Exception exception = new Exception();
                A.CallTo(() => _statusService.GetAsync(null)).Throws(exception);

                Func<Task<ActionResult<IEnumerable<Status>>>> action = _controller.
                    Awaiting(x => x.Get(null));
                await action.Should().ThrowAsync<Exception>();

                A.CallTo(() => _exceptionLogger.Log(exception, nameof(StatusController), _logger))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class PostTest : StatusControllerTest
        {
            [Fact]
            public async void Should_returnCreatedAtAction_when_postingStatus()
            {
                StatusDto statusDto = new StatusDto();

                ActionResult<Status> result = await _controller.Post(_deliveryId, statusDto);

                result.Result.Should().BeOfType(typeof(CreatedAtActionResult));
            }

            [Fact]
            public async void Should_useServiceWhen_when_postingStatus()
            {
                StatusDto statusDto = new StatusDto
                {
                    UpdateTime = DateTime.Now,
                    Message = "tset sutats",
                };

                A.CallTo(() => _statusService.AddAsync(_delivery, statusDto)).Returns(new Status
                {
                    UpdateTime = statusDto.UpdateTime,
                    Message = statusDto.Message,
                });

                ActionResult<Status> returnCode = await _controller.Post(_deliveryId, statusDto);

                A.CallTo(() => _statusService.AddAsync(_delivery, statusDto))
                    .MustHaveHappenedOnceExactly();
                var result = returnCode.Result as CreatedAtActionResult;
                ((Status)result?.Value)?.UpdateTime.Should().Be(statusDto.UpdateTime);
                ((Status)result?.Value)?.Message.Should().Be(statusDto.Message);
            }

            [Fact]
            public async void Should_log_when_exceptionIsThrowDuringPost()
            {
                Exception exception = new Exception();
                A.CallTo(() => _statusService.AddAsync(_delivery, null))
                    .Throws(exception);
                Func<Task<ActionResult<Status>>> action = _controller
                    .Awaiting(x => x.Post(_deliveryId, null));
                await action.Should().ThrowAsync<Exception>();

                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(StatusController), _logger))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnBadRequest_when_deliveryCouldNotBeFound()
            {
                int id = 123;
                EntityNotFoundException exception = new EntityNotFoundException(nameof(Delivery), id);
                A.CallTo(() => _deliveryService.GetAsync(id)).Throws(exception);

                ActionResult<Status> result = await _controller.Post(id, new StatusDto());

                result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            }
        }

        public class DeleteTest : StatusControllerTest
        {
            [Fact]
            public void Should_useService_when_deleting()
            {
                int id = 645;

                _controller.Delete(id);

                A.CallTo(() => _statusService.DeleteAsync(id))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnNotFound_when_serviceThrowsNotFoundException()
            {
                int id = 231;
                EntityNotFoundException exception = new EntityNotFoundException(nameof(Status), id);
                A.CallTo(() => _statusService.DeleteAsync(id)).Throws(exception);

                var result = await _controller.Delete(id);

                result.Should().BeOfType(typeof(NotFoundObjectResult));
            }

            [Fact]
            public async void Should_log_when_serviceThrowsExceptionOnDelete()
            {
                int id = 98;
                Exception exception = new Exception();
                A.CallTo(() => _statusService.DeleteAsync(id)).Throws(exception);

                Func<Task<ActionResult>> action = _controller.Awaiting(x => x.Delete(id));
                await action.Should().ThrowAsync<Exception>();
                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(StatusController), _logger))
                    .MustHaveHappenedOnceExactly();
            }
        }

        public class PutTest : StatusControllerTest
        {
            [Fact]
            public void Should_callService_when_updatingStatus()
            {
                int id = 564;
                StatusDto statusDto = new StatusDto();

                _controller.Put(id, statusDto, _deliveryId);

                A.CallTo(() => _statusService.UpdateAsync(id, statusDto, _delivery))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_log_when_serviceThrowsExceptionOnPut()
            {
                int id = 5;
                StatusDto statusDto = new StatusDto();
                Exception exception = new Exception();
                A.CallTo(() => _statusService.UpdateAsync(id, statusDto, _delivery))
                    .Throws(exception);

                Func<Task<ActionResult<Status>>> action =
                    _controller.Awaiting(x => x.Put(id, statusDto, _deliveryId));

                await action.Should().ThrowAsync<Exception>();
                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(StatusController), _logger))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnCreated_when_putCreatesNewEntity()
            {
                int id = 9135319;
                StatusDto statusDto = new StatusDto
                {
                    UpdateTime = DateTime.Now.AddDays(2),
                    Message = "Delivered test status",
                };
                Status status = new Status
                {
                    Id = id,
                    Message = statusDto.Message,
                    UpdateTime = statusDto.UpdateTime,
                };
                EntityNotFoundException exception = new EntityNotFoundException(nameof(status), id);
                A.CallTo(() => _statusService.GetAsync(id)).Throws(exception);
                A.CallTo(() => _statusService.UpdateAsync(id, statusDto, _delivery)).Returns(status);

                ActionResult<Status> actionResult = await _controller.Put(id, statusDto, _deliveryId);

                actionResult.Result.Should().BeOfType(typeof(CreatedAtActionResult));
                var result = actionResult.Result as CreatedAtActionResult;
                ((Status)result?.Value)?.Message.Should().Be(statusDto.Message);
                ((Status)result?.Value)?.UpdateTime.Should().Be(statusDto.UpdateTime);
            }

            [Fact]
            public async void Should_returnOk_when_putOverwritesExistingEntity()
            {
                int id = 4;
                StatusDto statusDto = new StatusDto
                {
                    Message = "Afleveret æøå",
                    UpdateTime = DateTime.Now,
                };
                Status status = new Status
                {
                    Id = id,
                    Message = statusDto.Message,
                    UpdateTime = statusDto.UpdateTime,
                };
                Status oldStatus = new Status
                {
                    Id = id,
                };
                A.CallTo(() => _statusService.GetAsync(id))
                    .Returns(new List<Status>
                    {
                        oldStatus,
                    });
                A.CallTo(() => _statusService.UpdateAsync(id, statusDto, _delivery)).Returns(status);

                ActionResult<Status> actionResult = await _controller.Put(id, statusDto, _deliveryId);

                actionResult.Result.Should().BeOfType(typeof(OkObjectResult));
                var result = actionResult.Result as CreatedAtActionResult;
                ((Status)result?.Value)?.Message.Should().Be(statusDto.Message);
                ((Status)result?.Value)?.UpdateTime.Should().Be(statusDto.UpdateTime);
            }
        }
    }
}