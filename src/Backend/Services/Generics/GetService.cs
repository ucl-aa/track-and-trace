using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Exceptions;
using Backend.Persistency;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Generics
{
    public class GetService<T>
        where T : class
    {
        private readonly DbContext _context;

        public GetService(TrackAndTraceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAsync(int? id)
        {
            return id.HasValue ? await GetSpecificModelAsync(id.Value) : await GetModelsAsync();
        }

        private async Task<IEnumerable<T>> GetSpecificModelAsync(int id)
        {
            T model = await _context.FindAsync<T>(id);

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