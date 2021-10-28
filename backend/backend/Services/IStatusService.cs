using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DataTransferObjects;
using Backend.DataTransferObjects;
using Backend.Models;

namespace backend.Services
{


    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetAsync(int? id);

        Task<Status> AddAsync(StatusDto statusDto);

        Task DeleteAsync(int id);

        Task<Status> UpdateAsync(int id, StatusDto statusDto);
    }
}
