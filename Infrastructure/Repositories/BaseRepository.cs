using Application.Interfaces;
using Common.Interfaces;
using Common.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> :
        IBaseRepository<T>,
        IReadRepository<T>
        where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Query(bool includeSoftDeleted = false, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (!includeSoftDeleted && typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
            {
                query = query.Cast<ISoftDeletable>()
                             .Where(e => !e.IsDeleted)
                             .Cast<T>();
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public async Task<T?> GetByIdAsync(
            Guid id,
            bool includeSoftDeleted = false,
            bool asNoTracking = true)
        {
            return await Query(includeSoftDeleted, asNoTracking)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Remove(T entity)
        {
            if (entity is ISoftDeletable deletable)
            {
                deletable.IsDeleted = true;
                Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }
    }
}
