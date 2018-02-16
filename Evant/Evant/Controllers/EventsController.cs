using System;
using System.Linq;
using Evant.Contracts.DataTransferObjects.Event;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/events")]
    public class EventsController : BaseController
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<FriendOperation> _friendOperationRepo;


        public EventsController(IRepository<Event> eventRepo, 
            IRepository<FriendOperation> friendOperationRepo)
        {
            _eventRepo = eventRepo;
            _friendOperationRepo = friendOperationRepo;
        }


        [Authorize]
        [HttpGet("{userId}")]
        public IActionResult GetUserEvents([FromRoute] Guid userId)
        {
            var events = _eventRepo.Where(e => e.UserId == userId).Select(e => new EventInfoDTO()
            {
                EventId = e.Id,
                Title = e.Title,
                Start = e.StartDate,
                PhotoUrl = e.Photo,
                User = new UserInfoDTO()
                {
                    UserId = e.User.Id,
                    FirstName = e.User.FirstName,
                    LastName = e.User.LastName,
                    PhotoUrl = e.User.Photo
                }
            }).ToList();

            if (events.IsNullOrEmpty())
            {
                return NotFound("Kayıt bulunamadı.");
            }
            else
            {
                return Ok(events);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("categoryevents/{categoryId}")]
        public IActionResult GetEventsByCategory([FromRoute] Guid categoryId)
        {
            var events = _eventRepo.Where(e => e.CategoryId == categoryId).Select(e => new EventInfoDTO()
            {
                EventId = e.Id,
                Title = e.Title,
                Start = e.StartDate,
                PhotoUrl = e.Photo,
                User = new UserInfoDTO()
                {
                    UserId = e.User.Id,
                    FirstName = e.User.FirstName,
                    LastName = e.User.LastName,
                    PhotoUrl = e.User.Photo
                }
            }).ToList();

            if (events.IsNullOrEmpty())
            {
                return NotFound("Kayıt bulunamadı.");
            }
            else
            {
                return Ok(events);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddEvent([FromBody] EventDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Eksik bilgi girdiniz.");

            Guid userId = User.GetUserId();
            var newEvent = new Event()
            {
                Id = new Guid(),
                UserId = userId,
                CategoryId = model.CategoryId,
                Title = model.Title,
                Description = model.Description,
                IsPrivate = model.IsPrivate,
                StartDate = model.StartAt,
                FinishDate = model.FinishAt,
                Photo = "test_photo",
                City = model.City,
                Town = model.Town,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };

            var response = _eventRepo.Insert(newEvent);
            if (response)
            {
                return Ok("Etkinlik oluşturuldu.");
            }
            else
            {
                return BadRequest("Etkinlik oluşturulamadı.");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent([FromRoute] Guid id)
        {
            var selectedEvent = _eventRepo.First(e => e.Id == id);
            if (selectedEvent == null)
                return NotFound("Etkinlik bulunamadı.");

            var response = _eventRepo.Delete(selectedEvent);
            if (response)
            {
                return Ok("Etkinlik silindi.");
            }
            else
            {
                return BadRequest("Etkinlik silinemedi.");
            }

        }

    }
}
