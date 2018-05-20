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
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Include(t => t.EventComments)
                .Include(t => t.EventOperations)
                .Include(t => t.User.Followers)
                .Where(t => (t.User.IsActive && !t.IsDeleted) && (t.UserId == userId || t.User.Followers.FirstOrDefault(f => f.FollowerUserId == userId) != null))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Event> EventDetail(Guid eventId)
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Include(t => t.EventComments)
                .Include(t => t.EventOperations)
                .FirstOrDefaultAsync(t => t.Id == eventId && !t.IsDeleted && t.User.IsActive);
        }

        public async Task<List<Event>> UserEvents(Guid userId)
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Event>> SimilarEvents(Event @event)
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Where(t => (t.CategoryId == @event.CategoryId || t.UserId == @event.UserId) && t.Id != @event.Id && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Event>> EventsByCategory(Guid categoryId)
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Where(t => t.CategoryId == categoryId && !t.IsDeleted && t.User.IsActive)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Event>> Search(string query)
        {
            return await Table
                .Include(t => t.Category)
                .Include(t => t.User)
                .Include(t => t.EventComments)
                .Include(t => t.EventOperations)
                .Where(t => !t.IsDeleted && (t.Town.ToLower().Contains(query.ToLower()) || t.City.ToLower().Contains(query.ToLower()) || t.Description.ToLower().Contains(query.ToLower()) || t.Title.ToLower().Contains(query.ToLower())))
                .OrderByDescending(t => t.CreatedAt)
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
