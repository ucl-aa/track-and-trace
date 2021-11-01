using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Backend.Services.Generics
{
    public class AddService<T>
        where T : class
    {
        private readonly DbContext _context;

        public AddService(DbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            EntityEntry<T> returnEntity = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return returnEntity.Entity;
        }
    }
}