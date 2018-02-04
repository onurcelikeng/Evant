using System;
using System.Linq;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/eventpperaitons")]
    public class EventOperationsController : BaseController
    {
        private readonly IRepository<EventOperation> _eventOperationRepo;


        public EventOperationsController(IRepository<EventOperation> eventOperationRepo)
        {
            _eventOperationRepo = eventOperationRepo;
        }


        [Authorize]
        [HttpGet("{eventId}")]
        public IActionResult GetEventUsers([FromRoute] Guid eventId)
        {
            var users = _eventOperationRepo.Where(eo => eo.EventId == eventId).Select(u => new UserInfoDTO()
            {
                UserId = u.User.Id,
                FirstName = u.User.FirstName,
                LastName = u.User.LastName,
                PhotoUrl = u.User.Photo
            });

            return Ok(users);
        }

        [Authorize]
        [HttpPost("{eventId}")]
        public IActionResult JoinEvent([FromRoute] Guid eventId)
        {
            Guid userId = User.GetUserId();

            var eventOperation = new EventOperation()
            {
                Id = new Guid(),
                UserId = userId,
                EventId = eventId
            };

            var response = _eventOperationRepo.Insert(eventOperation);
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
        public IActionResult LeaveEvent([FromRoute] Guid eventOperationId)
        {
            var selectedEventOperation = _eventOperationRepo.First(eo => eo.Id == eventOperationId);
            if(selectedEventOperation == null)
                return NotFound("Etkinliğe daha önce katılmadınız.");

            var response = _eventOperationRepo.Delete(selectedEventOperation);
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
