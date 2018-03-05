using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class NotificationsController : Controller
    {
        private readonly IRepository<Notification> _notificationRepo;
        private ILogHelper _logHelper;


        public NotificationsController(IRepository<Notification> notificationRepo,
            ILogHelper logHelper)
        {
            _notificationRepo = notificationRepo;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                Guid userId = User.GetUserId();
                var notifications = (await _notificationRepo.Where(n => n.ReceiverUserId == userId)).ToList();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logHelper.Log("Notifications", 500, "GetNotifications", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("read")]
        public async Task<IActionResult> ReadAllNotifications()
        {
            try
            {
                Guid userId = User.GetUserId();

                var unreadNotifications = (await _notificationRepo.Where(n => n.ReceiverUserId == userId && !n.IsRead));
                if (!unreadNotifications.IsNullOrEmpty())
                {
                    foreach (var notification in unreadNotifications)
                    {
                        notification.IsRead = true;
                        notification.UpdateAt = DateTime.Now;

                        var response = await _notificationRepo.Update(notification);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logHelper.Log("Notifications", 500, "ReadAllNotifications", ex.Message);
                return null;
            }
        }

    }
}
