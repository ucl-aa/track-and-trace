using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Backend.Models;

namespace Backend.Services
{
    public interface IDropOffService
    {
        Task<IEnumerable<DropOff>> GetAsync(int? id);

        Task<DropOff> AddAsync(DropOffDto dropOffDto);

        Task DeleteAsync(int id);

        Task<DropOff> UpdateAsync(int id, DropOffDto dropOffDto);
    }
}