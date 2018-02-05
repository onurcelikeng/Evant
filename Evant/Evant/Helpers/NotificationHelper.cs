using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Interfaces;

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
            

        }
    }
}
