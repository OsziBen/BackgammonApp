using Domain.User;

namespace Application.Interfaces.Common
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, string roleName);
    }

}
