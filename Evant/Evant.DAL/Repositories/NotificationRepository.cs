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
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(DataContext dbContext) : base(dbContext)
        {

        }

        
        public async Task<List<Notification>> Notifications(Guid userId)
        {
            return await Table
                .Include(t => t.SenderUser)
                .Include(t => t.Comment)
                .Include(t => t.Event)
                .Where(t => t.ReceiverUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

    }
}
