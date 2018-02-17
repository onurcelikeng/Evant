using Evant.DAL.EF;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evant.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext dbContext) : base(dbContext)
        {

        }


        public async Task<User> Login(string email, string password)
        {
            return await Table
                .Include(t => t.Setting)
                .SingleOrDefaultAsync(t => t.Email == email && t.Password == password);
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await Table
                .Include(t => t.Setting)
                .SingleOrDefaultAsync(t => t.Id == userId);
        }

    }
}
