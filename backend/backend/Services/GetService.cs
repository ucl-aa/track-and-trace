using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class GetService<T>
        where T : class
    {
        private readonly DbContext _context;

        public GetService(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAsync(int? id)
        {
            return id.HasValue ? await GetSpecificModelAsync(id.Value) : await GetModelsAsync();
        }

        private async Task<IEnumerable<T>> GetSpecificModelAsync(int id)
        {
            T model = _context.Find<T>(id);

            if (model is null)
            {
                throw new EntityNotFoundException(nameof(T), id);
            }

            return new[] { model };
        }

        private async Task<IEnumerable<T>> GetModelsAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}