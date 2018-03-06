using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Event;
using Evant.Contracts.DataTransferObjects.Notification;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.Repositories.Interfaces;
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
        private readonly INotificationRepository _notificationRepo;
        private ILogHelper _logHelper;


        public NotificationsController(INotificationRepository notificationRepo,
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
                var notifications = (await _notificationRepo.Notifications(userId)).Select(n => new NotificationDTO()
                {
                    NotificationId = n.Id,
                    Content = n.Content,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    NotificationType = n.NotificationType,
                    User = new UserInfoDTO()
                    {
                        UserId = (Guid)n.SenderUserId,
                        FirstName = n.SenderUser.FirstName,
                        LastName = n.SenderUser.LastName,
                        PhotoUrl = n.SenderUser.Photo
                    },
                    Event = (n.Event == null) ? null : new EventShortDTO()
                    {
                        EventId = (Guid)n.EventId,
                        PhotoUrl = n.Event.Photo
                    }
                }).ToList();
                if (notifications.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logHelper.Log("NotificationsController", 500, "GetNotifications", ex.Message);
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
                _logHelper.Log("NotificationsController", 500, "ReadAllNotifications", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification([FromRoute] Guid notificationId)
        {
            try
            {
                var notification = await _notificationRepo.First(n => n.Id == notificationId);
                if (notification == null)
                    return NotFound("Kayıt bulunamadı.");

                var response = await _notificationRepo.Delete(notification);
                if (response)
                    return Ok("Bildirim silindi");

                else
                    return BadRequest("Bildirim silinemedi");
            }
            catch (Exception ex)
            {
                _logHelper.Log("NotificationsController", 500, "DeleteNotification", ex.Message);
                return null;
            }
        }

    }
}
