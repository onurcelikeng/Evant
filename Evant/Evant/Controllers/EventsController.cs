using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Account;
using Evant.Contracts.DataTransferObjects.Event;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/events")]
    public class EventsController : BaseController
    {
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<Address> _addressRepo;


        public EventsController(IRepository<Event> eventRepo, IRepository<Address> addressRepo)
        {
            _eventRepo = eventRepo;
            _addressRepo = addressRepo;
        }


        [Authorize]
        [HttpPost]
        public IActionResult AddEvent([FromBody] EventDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Eksik bilgi girdiniz.");

            var addressId = new Guid();
            var newAddress = new Address()
            {
                AddressId = addressId,
                City = model.City,
                Town = model.Town,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };
            var result = _addressRepo.Insert(newAddress);

            Guid userId = User.GetUserId();
            var newEvent = new Event()
            {
                Id = new Guid(),
                UserId = userId,
                AddressId = addressId,
                CategoryId = model.CategoryId,
                Title = model.Title,
                Description = model.Description,
                IsPrivate = model.IsPrivate,
                StartDate = model.StartAt,
                FinishDate = model.FinishAt,
                Photo = "test_photo",
            };
            var response = _eventRepo.Insert(newEvent);
            if(response)
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

            var addressResponse = _addressRepo.Delete(selectedEvent.EventAddress);
            if (addressResponse)
            {
                var eventResponse = _eventRepo.Delete(selectedEvent);
                if (eventResponse)
                {
                    return Ok("Etkinlik ve adresi silindi.");
                }
                else
                {
                    return BadRequest("Etkinlik silinemedi.");
                }
            }
            else
            {
                return BadRequest("Etkinlik adresi silinemedi.");
            }
        }

    }
}
