using System.Threading.Tasks;
using Backend.Exceptions;
using Backend.Persistency;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Generics
{
    public class DeleteService<T>
        where T : class
    {
        private readonly DbContext _context;

        public DeleteService(TrackAndTraceContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            T model = await _context.FindAsync<T>(id);

            if (model is not null)
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundException(nameof(T), id);
            }
        }
    }
}