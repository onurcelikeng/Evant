using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/eventoperations")]
    public class EventOperationsController : BaseController
    {
        private readonly IEventOperationRepository _eventOperationRepo;


        public EventOperationsController(IEventOperationRepository eventOperationRepo)
        {
            _eventOperationRepo = eventOperationRepo;
        }


        [Authorize]
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventUsers([FromRoute] Guid eventId)
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

        [Authorize]
        [HttpPost("{eventId}")]
        public async Task<IActionResult> JoinEvent([FromRoute] Guid eventId)
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

        [Authorize]
        [HttpDelete("{eventOperationId}")]
        public async Task<IActionResult> LeaveEvent([FromRoute] Guid eventOperationId)
        {
            var selectedEventOperation = await _eventOperationRepo.First(eo => eo.Id == eventOperationId);
            if(selectedEventOperation == null)
                return NotFound("Kayıt bulunamadı.");

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
}
