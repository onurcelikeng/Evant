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


        public async Task<List<Event>> Timeline()
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Include(t => t.EventComments)
                .Include(t => t.EventOperations)
                .Where(t => t.IsDeleted == false)
                .ToListAsync();
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

        public async Task<List<Event>> Search(string query)
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Include(t => t.EventComments)
                .Include(t => t.EventOperations)
                .Where(t => t.IsDeleted == false && (t.Description.ToLower().Contains(query.ToLower()) || t.Title.ToLower().Contains(query.ToLower())))
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
