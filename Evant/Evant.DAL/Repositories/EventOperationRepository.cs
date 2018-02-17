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
    public class EventOperationRepository : Repository<EventOperation>, IEventOperationRepository
    {
        public EventOperationRepository(DataContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<EventOperation>> Participants(Guid eventId)
        {
            return await Table
                .Include(t => t.User)
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

        public async Task<bool> Attend(EventOperation entity)
        {
            try
            {
                await Table.AddAsync(entity);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Leave(EventOperation entity)
        {
            return await this.Delete(entity);
        }

    }
}
