using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Backend.Models;

namespace Backend.Services
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetAsync(int? id);

        Task<Status> AddAsync(StatusDto statusDto);

        Task DeleteAsync(int id);

        Task<Status> UpdateAsync(int id, StatusDto statusDto);
    }
}