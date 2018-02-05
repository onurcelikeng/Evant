using Evant.DAL.EF.Tables;

namespace Evant.Auth
{
    public interface IJwtFactory
    {
        string GenerateJwtToken(User user);
    }
}
