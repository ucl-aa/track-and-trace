﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Backend.Exceptions;
using Backend.Loggers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZipCodeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IZipCodeService _zipCodeService;
        private readonly IExceptionLogger _exceptionLogger;

        public ZipCodeController(
            IZipCodeService zipCodeService,
            IExceptionLogger exceptionLogger,
            ILogger logger)
        {
            _zipCodeService = zipCodeService;
            _exceptionLogger = exceptionLogger;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZipCode>>> Get(int? id)
        {
            try
            {
                return Ok(await _zipCodeService.GetAsync(id));
            }
            catch (EntityNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(ZipCodeController), _logger);
                throw;
            }
        }

        public async Task<ActionResult<ZipCode>> Post(ZipCodeDto zipCodeDto)
        {
            _zipCodeService.AddAsync(zipCodeDto);
            return CreatedAtAction(nameof(Post), new ZipCode());
        }
    }
}