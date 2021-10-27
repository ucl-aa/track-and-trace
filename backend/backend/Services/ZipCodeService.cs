using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Backend.Models;
using Backend.Persistency;

namespace Backend.Services
{
    public class ZipCodeService : IZipCodeService
    {
        private readonly GetService<ZipCode> _getService;
        private readonly DeleteService<ZipCode> _deleteService;

        public ZipCodeService(TrackAndTraceContext context)
        {
            _getService = new GetService<ZipCode>(context);
            _deleteService = new DeleteService<ZipCode>(context);
        }

        public async Task<IEnumerable<ZipCode>> GetAsync(int? id)
        {
            return await _getService.GetAsync(id);
        }

        public Task<ZipCode> AddAsync(ZipCodeDto zipCodeDto)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            await _deleteService.DeleteAsync(id);
        }

        public Task<ZipCode> UpdateAsync(int id, ZipCodeDto zipCodeDto)
        {
            throw new System.NotImplementedException();
        }
    }
}