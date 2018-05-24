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
using Evant.Storage.Extensions;
using Evant.Storage.Interfaces;
using Evant.Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Evant.Constants.GameConstant;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/events")]
    public class EventsController : BaseController
    {
        private readonly IEventRepository _eventRepo;
        private readonly ISearchHelper _searchHelper;
        private readonly IGameHelper _gameHelper;
        private readonly ILogHelper _logHelper;
        private readonly IAzureBlobStorage _blobStorage;


        public EventsController(IEventRepository eventRepo,
            ISearchHelper searchHelper,
            IGameHelper gameHelper,
            ILogHelper logHelper,
            IAzureBlobStorage blobStorage)
        {
            _eventRepo = eventRepo;
            _searchHelper = searchHelper;
            _gameHelper = gameHelper;
            _logHelper = logHelper;
            _blobStorage = blobStorage;
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

        [HttpGet("{eventId}/details")]
        public async Task<IActionResult> EventDetail(Guid eventId)
        {
            try
            {
                var @event = await _eventRepo.EventDetail(eventId);
                if (@event == null)
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    var model = new EventDetailDTO()
                    {
                        EventId = @event.Id,
                        Title = @event.Title,
                        Description = @event.Description,
                        Start = @event.StartDate,
                        Finish = @event.FinishDate,
                        PhotoUrl = @event.Photo,
                        TotalComments = @event.EventComments.Count,
                        TotalGoings = @event.EventOperations.Count,
                        IsPrivate = @event.IsPrivate,
                        Category = new CategoryInfoDTO()
                        {
                            CategoryId = @event.Category.Id,
                            Name = @event.Category.Name,
                            IconUrl = @event.Category.Icon
                        },
                        User = new UserInfoDTO()
                        {
                            UserId = @event.User.Id,
                            FirstName = @event.User.FirstName,
                            LastName = @event.User.LastName,
                            PhotoUrl = @event.User.Photo
                        },
                        Address = new AddressInfoDTO()
                        {
                            City = @event.City,
                            Town = @event.Town,
                            Latitude = @event.Latitude,
                            Longitude = @event.Longitude
                        }
                    };

                    return Ok(model);
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
        public async Task<IActionResult> UserEvents([FromRoute] Guid userId)
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
                    return NotFound("Kayıt bulunamadı.");

                return Ok(events);
            }
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "GetUserEvents", ex.Message);
                return null;
            }
        }

        [HttpGet("{eventId}/similar")]
        public async Task<IActionResult> SimilarEvents([FromRoute] Guid eventId)
        {
            try
            {
                var @event = await _eventRepo.EventDetail(eventId);
                if (@event == null)
                    return NotFound("Kayıt bulunamadı.");

                var events = (await _eventRepo.SimilarEvents(@event)).Select(e => new EventInfoDTO()
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
                    return NotFound("Kayıt bulunamadı.");

                return Ok(events);
            }
            catch (Exception ex)
            {
                _logHelper.Log("Events", 500, "GetSimilarEvents", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet("categoryevents/{categoryId}")]
        public async Task<IActionResult> EventsByCategory([FromRoute] Guid categoryId)
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
        [HttpGet("search/{query}")]
        public async Task<IActionResult> SearchEvents(string query)
        {
            try
            {
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
                    return BadRequest("Eksik bilgi girdiniz.");

                Guid userId = User.GetUserId();
                var entity = new Event()
                {
                    Id = model.EventId,
                    UserId = userId,
                    CategoryId = model.CategoryId,
                    Title = model.Title,
                    Description = model.Description,
                    IsPrivate = model.IsPrivate,
                    StartDate = model.StartAt,
                    FinishDate = model.FinishAt,
                    Photo = "https://evantstorage.blob.core.windows.net/events/" + model.EventId,
                    City = model.City,
                    Town = model.Town,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };

                var response = await _eventRepo.Add(entity);
                if (response)
                {
                    await _gameHelper.Point(userId, GameType.CreateEvent);
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

        [HttpPost]
        [Route("photo")]
        public async Task<IActionResult> UploadPhoto([FromForm] FileInputModel inputModel)
        {
            try
            {
                if (inputModel == null)
                    return BadRequest("Argument null");

                if (inputModel.File == null || inputModel.File.Length == 0)
                    return BadRequest("file not selected");

                var blobName = Guid.NewGuid().ToString();
                var fileStream = await inputModel.File.GetFileStream();

                var isUploaded = await _blobStorage.UploadAsync("event", blobName, fileStream);
                if (isUploaded)
                {
                    return Ok(blobName);
                }
                else
                {
                    return BadRequest("photo not uploaded.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("EventsController", 500, "UploadPhoto", ex.Message);
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
                    var entity = new Event()
                    {
                        Id = model.EventId,
                        UserId = User.GetUserId(),
                        CategoryId = model.CategoryId,
                        Title = model.Title,
                        Description = model.Description,
                        IsPrivate = model.IsPrivate,
                        StartDate = model.StartAt,
                        FinishDate = model.FinishAt,
                        Photo = "https://evantstorage.blob.core.windows.net/events/" + model.EventId,
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

        [HttpGet("city")]
        public async Task<IActionResult> CityEvents()
        {
            try
            {
                var events = (await _eventRepo.CityEvents("izmir")).Select(e => new EventDetailDTO()
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
                return null;
            }
        }

        [HttpGet("town")]
        public async Task<IActionResult> TownEvents()
        {
            try
            {
                var events = (await _eventRepo.TownEvents("bornova")).Select(e => new EventDetailDTO()
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
                return null;
            }
        }

    }
}
