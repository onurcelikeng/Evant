using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Address;
using Evant.Contracts.DataTransferObjects.Category;
using Evant.Contracts.DataTransferObjects.Event;
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
    [Route("api/events")]
    public class EventsController : BaseController
    {
        private readonly IEventRepository _eventRepo;
        private readonly ISearchHelper _searchHelper;
        private readonly ILogHelper _logHelper;


        public EventsController(IEventRepository eventRepo,
            ISearchHelper searchHelper,
            ILogHelper logHelper)
        {
            _eventRepo = eventRepo;
            _searchHelper = searchHelper;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Timeline()
        {
            try
            {
                Guid userId = User.GetUserId();
                var events = (await _eventRepo.Timeline(userId)).Select(e => new EventDetailDTO()
                {
                    EventId = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Start = e.StartDate,
                    Finish = e.FinishDate,
                    PhotoUrl = e.Photo,
                    TotalComments = e.EventComments.Count,
                    TotalGoings = e.EventOperations.Count,
                    Category = new CategoryInfoDTO()
                    {
                        CategoryId = e.Category.Id,
                        Name = e.Category.Name
                    },
                    User = new UserInfoDTO()
                    {
                        UserId = e.User.Id,
                        FirstName = e.User.FirstName,
                        LastName = e.User.LastName,
                        PhotoUrl = e.User.Photo
                    },
                    Address = new AddressInfoDTO()
                    {
                        City = e.City,
                        Town = e.Town,
                        Latitude = e.Latitude,
                        Longitude = e.Longitude
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
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "Timeline", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserEvents([FromRoute] Guid userId)
        {
            try
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
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "GetUserEvents", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("categoryevents/{categoryId}")]
        public async Task<IActionResult> GetEventsByCategory([FromRoute] Guid categoryId)
        {
            try
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
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "GetEventsByCategory", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("search/{query}")]
        public async Task<IActionResult> SearchEvents(string query)
        {
            try
            {
                Guid userId = User.GetUserId();
                await _searchHelper.Add(userId, query);

                var events = (await _eventRepo.Search(query)).Select(e => new EventDetailDTO()
                {
                    EventId = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Start = e.StartDate,
                    Finish = e.FinishDate,
                    PhotoUrl = e.Photo,
                    TotalComments = e.EventComments.Count,
                    TotalGoings = e.EventOperations.Count,
                    Category = new CategoryInfoDTO()
                    {
                        CategoryId = e.Category.Id,
                        Name = e.Category.Name
                    },
                    User = new UserInfoDTO()
                    {
                        UserId = e.User.Id,
                        FirstName = e.User.FirstName,
                        LastName = e.User.LastName,
                        PhotoUrl = e.User.Photo
                    },
                    Address = new AddressInfoDTO()
                    {
                        City = e.City,
                        Town = e.Town,
                        Latitude = e.Latitude,
                        Longitude = e.Longitude
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
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "SearchEvents", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] EventDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Eksik bilgi girdiniz.");
                }

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
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "AddEvent", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody] EventDTO model)
        {
            try
            {
                var selectedEvent = await _eventRepo.First(e => e.Id == model.EventId);
                if (selectedEvent == null)
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    Guid userId = User.GetUserId();

                    var entity = new Event()
                    {
                        Id = (Guid)model.EventId,
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

                    var response = await _eventRepo.Update(entity);
                    if (response)
                    {
                        return Ok("Etkinlik güncellendi.");
                    }
                    else
                    {
                        return BadRequest("Etkinlik güncellenemedi.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "UpdateEvent", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid eventId)
        {
            try
            {
                var response = await _eventRepo.SoftDelete(eventId);
                if (response)
                {
                    return Ok("Etkinlik silindi.");
                }
                else
                {
                    return BadRequest("Etkinlik silinemedi.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "DeleteEvent", ex.Message);
                return null;
            }
        }

    }
}
