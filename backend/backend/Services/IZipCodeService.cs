using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataTransferObjects;
using Backend.Models;

namespace Backend.Services
{
    public interface IZipCodeService
    {
        Task<IEnumerable<ZipCode>> GetAsync(int? id);

        Task<ZipCode> AddAsync(ZipCodeDto zipCodeDto);
    }
}