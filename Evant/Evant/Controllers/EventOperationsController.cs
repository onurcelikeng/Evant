using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/eventoperations")]
    public class EventOperationsController : BaseController
    {
        private readonly IEventOperationRepository _eventOperationRepo;
        private readonly IEventRepository _eventRepo;
        private readonly INotificationHelper _notificationHelper;
        private ILogHelper _logHelper;


        public EventOperationsController(IEventOperationRepository eventOperationRepo,
            IEventRepository eventRepo,
            INotificationHelper notificationHelper,
            ILogHelper logHelper)
        {
            _eventOperationRepo = eventOperationRepo;
            _eventRepo = eventRepo;
            _notificationHelper = notificationHelper;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventUsers([FromRoute] Guid eventId)
        {
            try
            {
                var users = (await _eventOperationRepo.Participants(eventId)).Select(u => new UserInfoDTO()
                {
                    UserId = u.User.Id,
                    FirstName = u.User.FirstName,
                    LastName = u.User.LastName,
                    PhotoUrl = u.User.Photo
                }).ToList();

                if (users.IsNullOrEmpty())
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("EventOperations", 500, "GetEventUsers", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet("status/{eventId}")]
        public async Task<IActionResult> IsJoin([FromRoute] Guid eventId)
        {
            try
            {
                Guid userId = User.GetUserId();

                var selectedEventOperation = await _eventOperationRepo.First(eo => eo.EventId == eventId && eo.UserId == userId);
                if (selectedEventOperation == null)
                {
                    return BadRequest("Katılmamışsın.");
                }
                else
                {
                    return Ok("Katılmışsın.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("EventOperations", 500, "IsFollow", ex.Message);
                return null;
            }

        }

        [Authorize]
        [HttpPost("{eventId}")]
        public async Task<IActionResult> JoinEvent([FromRoute] Guid eventId)
        {
            try
            {
                Guid userId = User.GetUserId();

                var selectedEventOperation = await _eventOperationRepo.First(eo => eo.UserId == userId && eo.EventId == eventId);
                if (selectedEventOperation != null)
                {
                    return BadRequest("Etkinliğe daha önce katıldınız.");
                }
                else
                {
                    var entity = new EventOperation()
                    {
                        Id = new Guid(),
                        UserId = userId,
                        EventId = eventId
                    };

                    var response = await _eventOperationRepo.Add(entity);
                    if (response)
                    {
                        var searchedEvent = await _eventRepo.First(e => e.Id == eventId);
                        if(searchedEvent != null)
                        {
                            Guid receiverId = searchedEvent.UserId;
                            await _notificationHelper.SendEventAttendNotification(userId, receiverId);
                        }

                        return Ok("Etkinliğe katıldınız.");
                    }
                    else
                    {
                        return BadRequest("Etkinliğe katılamadınız.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("EventOperations", 500, "JoinEvent", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> LeaveEvent([FromRoute] Guid eventId)
        {
            try
            {
                Guid userId = User.GetUserId();

                var selectedEventOperation = await _eventOperationRepo.First(eo => eo.EventId == eventId && eo.UserId == userId);
                if (selectedEventOperation == null)
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    var response = await _eventOperationRepo.Delete(selectedEventOperation);
                    if (response)
                    {
                        return Ok("Etkinlikten ayrıldınız.");
                    }
                    else
                    {
                        return BadRequest("Etkinlikten ayrılamadınız.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("EventOperations", 500, "LeaveEvent", ex.Message);
                return null;
            }
        }

    }
}
