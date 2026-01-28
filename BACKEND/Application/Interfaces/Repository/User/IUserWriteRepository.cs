namespace Application.Interfaces.Repository.User
{
    public interface IUserWriteRepository
    {
        Task<Domain.User.User?> GetByIdAsync(Guid id);
    }
}
