﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Event;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/events")]
    public class EventsController : BaseController
    {
        private readonly IEventRepository _eventRepo;


        public EventsController(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }


        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserEvents([FromRoute] Guid userId)
        {
            var events = (await _eventRepo.UserEvents(userId)).Select(e => new EventInfoDTO()
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
        public async Task<IActionResult> GetEventsByCategory([FromRoute] Guid categoryId)
        {
            var events = (await _eventRepo.EventsByCategory(categoryId)).Select(e => new EventInfoDTO()
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
        public async Task<IActionResult> AddEvent([FromBody] EventDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Eksik bilgi girdiniz.");

            Guid userId = User.GetUserId();
            var entity = new Event()
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

            var response = await _eventRepo.Add(entity);
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
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid id)
        {
            var selectedEvent = await _eventRepo.First(e => e.Id == id);
            if (selectedEvent == null)
                return NotFound("Kayıt bulunamadı.");

            var response = await _eventRepo.Delete(selectedEvent);
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
