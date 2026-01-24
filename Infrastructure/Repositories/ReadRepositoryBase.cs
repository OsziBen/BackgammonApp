using Common.Interfaces;
using Common.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class ReadRepositoryBase<T>
        where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        protected ReadRepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }

        protected IQueryable<T> Query()
        {
            IQueryable<T> query = _context.Set<T>()
                .AsNoTracking();

            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
            {
                query = query.Cast<ISoftDeletable>()
                             .Where(e => !e.IsDeleted)
                             .Cast<T>();
            }

            return query;
        }
    }
}
