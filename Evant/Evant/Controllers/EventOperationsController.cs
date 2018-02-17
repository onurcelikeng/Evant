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
        private ILogHelper _logHelper;


        public EventOperationsController(IEventOperationRepository eventOperationRepo,
            ILogHelper logHelper)
        {
            _eventOperationRepo = eventOperationRepo;
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
                _logHelper.Log("EventOperations", 500, ex.Message);
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

                var entity = new EventOperation()
                {
                    Id = new Guid(),
                    UserId = userId,
                    EventId = eventId
                };

                var response = await _eventOperationRepo.Add(entity);
                if (response)
                {
                    return Ok("Etkinliğe katıldınız.");
                }
                else
                {
                    return BadRequest("Etkinliğe katılamadınız.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("EventOperations", 500, ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpDelete("{eventOperationId}")]
        public async Task<IActionResult> LeaveEvent([FromRoute] Guid eventOperationId)
        {
            try
            {
                var selectedEventOperation = await _eventOperationRepo.First(eo => eo.Id == eventOperationId);
                if (selectedEventOperation == null)
                {
                    return NotFound("Kayıt bulunamadı.");
                }

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
            catch (Exception ex)
            {
                _logHelper.Log("EventOperations", 500, ex.Message);
                return null;
            }
        }

    }
}
