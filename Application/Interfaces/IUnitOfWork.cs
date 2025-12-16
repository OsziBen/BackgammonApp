using Domain.AppRole;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IBaseRepository<AppRole> AppRoles { get; }
        IGroupMembershipRoleRepository GroupMembershipRoles { get; }
        // repositories

        Task<int> CommitAsync();
        Task RollbackAsync();
    }
}
