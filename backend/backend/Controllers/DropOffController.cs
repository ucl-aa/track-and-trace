using System;
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
    public class DropOffController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IDropOffService _zipCodeService;
        private readonly IExceptionLogger _exceptionLogger;

        public DropOffController(
            IDropOffService zipCodeService,
            IExceptionLogger exceptionLogger,
            ILogger logger)
        {
            _zipCodeService = zipCodeService;
            _exceptionLogger = exceptionLogger;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DropOff>>> Get(int? id)
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
                _exceptionLogger.Log(exception, nameof(DropOffController), _logger);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<DropOff>> Post(DropOffDto zipCodeDto)
        {
            try
            {
                DropOff result = await _zipCodeService.AddAsync(zipCodeDto);
                return CreatedAtAction(nameof(Post), result);
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(DropOffController), _logger);
                throw;
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _zipCodeService.DeleteAsync(id);
                return NoContent();
            }
            catch (EntityNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(DropOffController), _logger);
                throw;
            }
        }

        [HttpPut]
        public async Task<ActionResult<DropOff>> Put(int id, DropOffDto zipCodeDto)
        {
            try
            {
                return await PutDropOff(id, zipCodeDto);
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(DropOffController), _logger);
                throw;
            }
        }

        private async Task<ActionResult<DropOff>> PutDropOff(int id, DropOffDto zipCodeDto)
        {
            try
            {
                await _zipCodeService.GetAsync(id);
                return Ok(await _zipCodeService.UpdateAsync(id, zipCodeDto));
            }
            catch (EntityNotFoundException)
            {
                return CreatedAtAction(nameof(Put), await _zipCodeService.UpdateAsync(id, zipCodeDto));
            }
        }
    }
}