using System;
using System.Linq;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/notifications")]
    public class NotificationsController : BaseController
    {
        private readonly IRepository<Notification> _notificationRepo;
        private readonly INotificationHelper _notificationHelper;


        public NotificationsController(IRepository<Notification> notificationRepo,
            INotificationHelper notificationHelper)
        {
            _notificationRepo = notificationRepo;
            _notificationHelper = notificationHelper;
        }


        //[Authorize]
        [HttpGet]
        public IActionResult GetNotifications()
        {
            //Guid userId = User.GetUserId();
            _notificationHelper.SendNotification();
            //var notifications = _notificationRepo.Where(n => n.UserId == userId);

            return Ok();
        }

        [Authorize]
        [HttpPut]
        public IActionResult ReadAllNotifications()
        {
            Guid userId = User.GetUserId();

            var notifications = _notificationRepo.Where(n => n.UserId == userId && n.IsRead == false).ToList();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.UpdateAt = DateTime.Now;

                var response = _notificationRepo.Update(notification);
            }

            return Ok();
        }

    }
}
