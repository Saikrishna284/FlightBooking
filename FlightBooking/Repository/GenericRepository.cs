using FlightBooking.Data;
using FlightBooking.Interface;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly FlightDbContext _context;
        private readonly DbSet<T> table;

        public GenericRepository(FlightDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }
            await table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await table.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            table.Remove(entity);
            await _context.SaveChangesAsync();
           
        }

       

       public async Task UpdateAsync(T entity)
        {
            table.Update(entity);
            await _context.SaveChangesAsync();
           
        }


        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await table.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities) // Bulk insert
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await table.CountAsync();
        }
    }
}