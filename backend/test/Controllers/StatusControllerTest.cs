using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Controllers;
using Backend.Exceptions;
using Backend.Loggers;
using Backend.Models;
using backend.Services;
using Backend.Services;
using Backend.DataTransferObjects;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using backend.DataTransferObjects;

namespace test.Controllers
{

    public class StatusControllerTest
    {
        private readonly IStatusService _statusService;
        private readonly StatusController _controller;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly ILogger _logger;

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

            _exceptionLogger = A.Fake<IExceptionLogger>();
            _statusService = A.Fake<IStatusService>();
            _logger = new LoggerFactory().CreateLogger("test logger");
            A.CallTo(() => _statusService.GetAsync(null)).Returns(statuses);
            _controller = new StatusController(
                _statusService,
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

                ActionResult<Status> result = await _controller.Post(statusDto);

                result.Result.Should().BeOfType(typeof(CreatedAtActionResult));
            }

            [Fact]
            public async void Should_useServiceWhen_when_postingStatus()
            {
                StatusDto StatusDto = new StatusDto
                {
                    UpdateTime = DateTime.Now,
                    Message = "tset sutats"
                };
                A.CallTo(() => _statusService.AddAsync(StatusDto)).Returns(new Status
                {
                    UpdateTime = StatusDto.UpdateTime,
                    Message = StatusDto.Message,
                });

                ActionResult<Status> returnCode = await _controller.Post(StatusDto);

                A.CallTo(() => _statusService.AddAsync(StatusDto))
                    .MustHaveHappenedOnceExactly();
                var result = returnCode.Result as CreatedAtActionResult;
                ((Status)result?.Value)?.UpdateTime.Should().Be(StatusDto.UpdateTime);
                ((Status)result?.Value)?.Message.Should().Be(StatusDto.Message);
            }

            [Fact]
            public async void Should_log_when_exceptionIsThrowDuringPost()
            {
                Exception exception = new Exception();
                A.CallTo(() => _statusService.AddAsync(null))
                    .Throws(exception);
                Func<Task<ActionResult<Status>>> action = _controller
                    .Awaiting(x => x.Post(null));
                await action.Should().ThrowAsync<Exception>();

                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(StatusController), _logger))
                    .MustHaveHappenedOnceExactly();
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
                StatusDto StatusDto = new StatusDto();

                _controller.Put(id, StatusDto);

                A.CallTo(() => _statusService.UpdateAsync(id, StatusDto))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_log_when_serviceThrowsExceptionOnPut()
            {
                int id = 5;
                StatusDto StatusDto = new StatusDto();
                Exception exception = new Exception();
                A.CallTo(() => _statusService.UpdateAsync(id, StatusDto))
                    .Throws(exception);

                Func<Task<ActionResult<Status>>> action =
                    _controller.Awaiting(x => x.Put(id, StatusDto));

                await action.Should().ThrowAsync<Exception>();
                A.CallTo(() => _exceptionLogger
                        .Log(exception, nameof(StatusController), _logger))
                    .MustHaveHappenedOnceExactly();
            }

            [Fact]
            public async void Should_returnCreated_when_putCreatesNewEntity()
            {
                int id = 9135319;
                StatusDto StatusDto = new StatusDto
                {
                    UpdateTime = DateTime.Now.AddDays(2),
                    Message = "Delivered test status"
                };
                Status Status = new Status
                {
                    Id = id,
                    Message = StatusDto.Message,
                    UpdateTime = StatusDto.UpdateTime,
                };
                EntityNotFoundException exception = new EntityNotFoundException(nameof(Status), id);
                A.CallTo(() => _statusService.GetAsync(id)).Throws(exception);
                A.CallTo(() => _statusService.UpdateAsync(id, StatusDto)).Returns(Status);

                ActionResult<Status> actionResult = await _controller.Put(id, StatusDto);

                actionResult.Result.Should().BeOfType(typeof(CreatedAtActionResult));
                var result = actionResult.Result as CreatedAtActionResult;
                ((Status)result?.Value)?.Message.Should().Be(StatusDto.Message);
                ((Status)result?.Value)?.UpdateTime.Should().Be(StatusDto.UpdateTime);
            }

            [Fact]
            public async void Should_returnOk_when_putOverwritesExistingEntity()
            {
                int id = 4;
                StatusDto StatusDto = new StatusDto
                {
                    Message = "Afleveret æøå",
                    UpdateTime = DateTime.Now,
                };
                Status Status = new Status
                {
                    Id = id,
                    Message = StatusDto.Message,
                    UpdateTime = StatusDto.UpdateTime,
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
                A.CallTo(() => _statusService.UpdateAsync(id, StatusDto)).Returns(Status);

                ActionResult<Status> actionResult = await _controller.Put(id, StatusDto);

                actionResult.Result.Should().BeOfType(typeof(OkObjectResult));
                var result = actionResult.Result as CreatedAtActionResult;
                ((Status)result?.Value)?.Message.Should().Be(StatusDto.Message);
                ((Status)result?.Value)?.UpdateTime.Should().Be(StatusDto.UpdateTime);
            }
        }
    }
}
