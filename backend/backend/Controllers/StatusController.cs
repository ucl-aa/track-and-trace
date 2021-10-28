using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Loggers;
using Backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Backend.Exceptions;
using backend.DataTransferObjects;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IStatusService _statusService;
        private readonly IExceptionLogger _exceptionLogger;

        public StatusController(
            IStatusService statusService,
            IExceptionLogger exceptionLogger,
            ILogger logger)
        {
            _statusService = statusService;
            _exceptionLogger = exceptionLogger;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> Get(int? id)
        {
            try
            {
                return Ok(await _statusService.GetAsync(id));
            }
            catch (EntityNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(StatusController), _logger);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Status>> Post(StatusDto statusDto)
        {
            try
            {
                Status result = await _statusService.AddAsync(statusDto);
                return CreatedAtAction(nameof(Post), result);
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(StatusController), _logger);
                throw;
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _statusService.DeleteAsync(id);
                return NoContent();
            }
            catch (EntityNotFoundException exception)
            {
                return NotFound(exception.Message);
                throw;
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(StatusController), _logger);
                throw;
            }
        }

        [HttpPut]
        public async Task<ActionResult<Status>> Put(int id, StatusDto statusDto)
        {
            try
            {
                return await PutStatus(id, statusDto);
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(StatusController), _logger);
                throw;
            }
        }

        private async Task<ActionResult<Status>> PutStatus(int id, StatusDto statusDto)
        {
            try
            {
                await _statusService.GetAsync(id);
                return Ok(await _statusService.UpdateAsync(id, statusDto));
            }
            catch (EntityNotFoundException)
            {
                return CreatedAtAction(nameof(Put), await _statusService.UpdateAsync(id, statusDto));
            }
        }

    }
}
