using System;
using System.Collections.Generic;
using System.Linq;
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
    public class StatusController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IStatusService _statusService;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IDeliveryService _deliveryService;

        public StatusController(
            IStatusService statusService,
            IDeliveryService deliveryService,
            IExceptionLogger exceptionLogger,
            ILogger logger)
        {
            _statusService = statusService;
            _deliveryService = deliveryService;
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
        public async Task<ActionResult<Status>> Post(int deliveryId, StatusDto statusDto)
        {
            try
            {
                return await PostStatus(deliveryId, statusDto);
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(StatusController), _logger);
                throw;
            }
        }

        private async Task<ActionResult<Status>> PostStatus(int deliveryId, StatusDto statusDto)
        {
            try
            {
                return await ExecutePostStatus(deliveryId, statusDto);
            }
            catch (EntityNotFoundException exception)
            {
                return BadRequest(exception);
            }
        }

        private async Task<ActionResult<Status>> ExecutePostStatus(int deliveryId, StatusDto statusDto)
        {
            var deliveries = await _deliveryService.GetAsync(deliveryId);
            Status result = await _statusService.AddAsync(deliveries.ToList()[0], statusDto);
            return CreatedAtAction(nameof(Post), result);
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
        public async Task<ActionResult<Status>> Put(int id, StatusDto statusDto, int deliveryId)
        {
            try
            {
                return await PutStatus(id, statusDto, deliveryId);
            }
            catch (Exception exception)
            {
                _exceptionLogger.Log(exception, nameof(StatusController), _logger);
                throw;
            }
        }

        private async Task<ActionResult<Status>> PutStatus(int id, StatusDto statusDto, int deliveryId)
        {
            IEnumerable<Delivery> deliveries = await _deliveryService.GetAsync(deliveryId);
            try
            {
                await _statusService.GetAsync(id);
                return Ok(await _statusService.UpdateAsync(id, statusDto, deliveries.ToList()[0]));
            }
            catch (EntityNotFoundException)
            {
                return CreatedAtAction(nameof(Put), await _statusService.UpdateAsync(id, statusDto, deliveries.ToList()[0]));
            }
        }
    }
}