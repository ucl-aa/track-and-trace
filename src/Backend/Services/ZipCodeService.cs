using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Backend.Models;
using Backend.Persistency;
using Backend.Services.Generics;

namespace Backend.Services
{
    public class ZipCodeService : IZipCodeService
    {
        private readonly GetService<ZipCode> _getService;
        private readonly DeleteService<ZipCode> _deleteService;
        private readonly AddService<ZipCode> _addService;

        public ZipCodeService(GetService<ZipCode> getService, DeleteService<ZipCode> deleteService, AddService<ZipCode> addService)
        {
            _getService = getService;
            _deleteService = deleteService;
            _addService = addService;
        }

        public async Task<IEnumerable<ZipCode>> GetAsync(int? id)
        {
            return await _getService.GetAsync(id);
        }

        public Task<ZipCode> AddAsync(ZipCodeDto zipCodeDto)
        {
            return _addService.AddAsync(zipCodeDto.GetZipCode());
        }

        public async Task DeleteAsync(int id)
        {
            await _deleteService.DeleteAsync(id);
        }

        public async Task<ZipCode> UpdateAsync(int id, ZipCodeDto zipCodeDto)
        {
            await _deleteService.DeleteAsync(id);
            return await _addService.AddAsync(zipCodeDto.GetZipCode());
        }
    }
}