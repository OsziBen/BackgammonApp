using Common.Models;

namespace Application.Interfaces
{
    public interface IReadRepository<T> where T : BaseEntity
    {
        IQueryable<T> Query(bool includeSoftDeleted = false, bool asNoTracking = true);
        Task<T?> GetByIdAsync(Guid id, bool includeSoftDeleted = false, bool asNoTracking = true);
    }
}
