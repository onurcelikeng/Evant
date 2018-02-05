using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evant.Helpers
{
    public class NotificationHelper : INotificationHelper
    {
        private readonly IRepository<Notification> _notificationRepo;


        public NotificationHelper(IRepository<Notification> notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }


        public void SendNotification()
        {
            var value = 0;

        }
    }
}
