using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;
using FluentAssertions;
using Backend.Models;
using Backend.Services;
using Backend.Loggers;
using Backend.Exceptions;


namespace test.Controllers
{
    internal class StatusControllerTest
    {
        private readonly IStatusService _statusService;
        private readonly StatusControllerTest _controller;
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

        }
    }
}
