using Application.Interfaces;
using Domain.AppRole;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private bool _disposed;
        private IDbContextTransaction? _currentTransaction;

        public IUserRepository Users { get; }
        public IBaseRepository<AppRole> AppRoles { get; }
        public IGroupMembershipRoleRepository GroupMembershipRoles { get; }
        // többi repo

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
            AppRoles = new BaseRepository<AppRole>(_context);
            GroupMembershipRoles = new GroupMembershipRoleRepository(_context);
            // többi repo
        }

        public async Task<int> CommitAsync()
        {
            if (_currentTransaction == null)
            {
                _currentTransaction = await _context.Database.BeginTransactionAsync();
            }

            try
            {
                int result = await _context.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;

                return result;
            }
            catch
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync();
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }

                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _currentTransaction?.Dispose();
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
