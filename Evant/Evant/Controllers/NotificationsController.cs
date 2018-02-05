using System;
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


        public NotificationsController(IRepository<Notification> notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetNotifications()
        {
            Guid userId = User.GetUserId();

            //var notifications = _notificationRepo.Where(n => n.UserId == userId);

            return Ok();
        }

        [Authorize]
        [HttpPut("{notificationId}")]
        public IActionResult ReadNotification([FromRoute] Guid notificationId)
        {
            var selectedNotification = _notificationRepo.First(n => n.Id == notificationId);
            if (selectedNotification == null)
            {
                return NotFound("Bildirim bulunamadı.");
            }
            else
            {
                selectedNotification.IsRead = true;
                selectedNotification.UpdateAt = DateTime.Now;
                
                var response = _notificationRepo.Update(selectedNotification);
                if (response)
                {
                    return Ok("Bildirim güncellendi.");
                }
                else
                {
                    return BadRequest("Bildirim güncellenemedi.");
                }
            }

        }

        [Authorize]
        [HttpDelete("{notificationId}")]
        public IActionResult DeleteNotification([FromRoute] Guid notificationId)
        {
            var notification = _notificationRepo.First(c => c.Id == notificationId);
            if (notification == null)
            {
                return NotFound("Bildirim bulunamadı.");
            }

            var response = _notificationRepo.Delete(notification);
            if (response)
            {
                return Ok("Bildirim silindi.");
            }
            else
            {
                return BadRequest("Bildirim silinemedi.");
            }
        }

    }
}
