using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Login(string email, string password);
        Task<User> GetUser(Guid userId);
    }
}
