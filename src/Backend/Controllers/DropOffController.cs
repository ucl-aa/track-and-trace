using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class DropOffController : ControllerBase
    {
        private readonly IDropOffService _dropOffService;

        public DropOffController(IDropOffService dropOffService)
        {
            _dropOffService = dropOffService;
        }

        public Task<IEnumerable<DropOff>> Get(int? id)
        {
            return _dropOffService.GetAsync(id);
        }
    }
}