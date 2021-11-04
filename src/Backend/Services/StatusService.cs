using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Backend.Models;
using Backend.Services.Generics;

namespace Backend.Services
{
    public class StatusService : IStatusService
    {
        private readonly GetService<Status> _getService;
        private readonly DeleteService<Status> _deleteService;
        private readonly AddService<Status> _addService;

        public StatusService(GetService<Status> getService, DeleteService<Status> deleteService, AddService<Status> addService)
        {
            _getService = getService;
            _deleteService = deleteService;
            _addService = addService;
        }

        public async Task<Status> AddAsync(Delivery delivery, StatusDto statusDto)
        {
            Status status = statusDto.GetStatus();
            delivery.StatusHistory.Add(status);
            return await _addService.AddAsync(status);
        }

        public async Task DeleteAsync(int id)
        {
            await _deleteService.DeleteAsync(id);
        }

        public async Task<IEnumerable<Status>> GetAsync(int? id)
        {
            return await _getService.GetAsync(id);
        }

        public async Task<Status> UpdateAsync(int id, StatusDto statusDto, Delivery delivery)
        {
            Status status = statusDto.GetStatus();
            delivery.StatusHistory.Add(status);
            await _deleteService.DeleteAsync(id);
            return await _addService.AddAsync(status);
        }
    }
}
