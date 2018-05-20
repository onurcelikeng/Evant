using Evant.DAL.EF;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .Include(t => t.Business)
                .Include(t => t.Followers)
                .Include(t => t.Followings)
                .SingleOrDefaultAsync(t => t.Id == userId);
        }

        public async Task<List<User>> Search(string query)
        {
            return await Table
                .Where(t => t.IsActive && (t.Email.ToLower().Contains(query.ToLower()) || t.FirstName.ToLower().Contains(query.ToLower()) || t.LastName.ToLower().Contains(query.ToLower())))
                .ToListAsync();
        }

        public async Task<bool> EmailCheck(string email)
        {
            return (await Table.CountAsync(t => t.Email == email)) > 0;
        }

    }
}
