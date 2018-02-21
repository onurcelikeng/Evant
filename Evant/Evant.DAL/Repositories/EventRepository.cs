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
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(DataContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<Event>> Timeline(Guid userId)
        {
            /*  
             select E.Title, E.CreatedAt
from Events as E 
join Users as U on E.UserId = U.Id
join FriendOperations as FO on FO.FollowingUserId = E.UserId
where FO.FollowerUserId = 'EE58B427-7B63-4D48-EE8D-08D577DCEF07'
order by E.CreatedAt desc  
             */
              
            //var result = Table.FromSql();
            return null;
        }

        public async Task<List<Event>> UserEvents(Guid userId)
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Where(t => t.UserId == userId && t.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<List<Event>> EventsByCategory(Guid categoryId)
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Where(t => t.CategoryId == categoryId && t.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<bool> SoftDelete(Guid eventId)
        {
            var data = await this.First(t => t.Id == eventId);
            if (data != null)
            {
                data.IsDeleted = true;
                return await this.Update(data);
            }
            return false;
        }
    }
}
