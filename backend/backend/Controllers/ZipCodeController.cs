using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Exceptions;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZipCodeController : ControllerBase
    {
        private readonly IZipCodeService _zipCodeService;

        public ZipCodeController(IZipCodeService zipCodeService)
        {
            _zipCodeService = zipCodeService;
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
        }
    }
}