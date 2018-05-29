using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Admin;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/admin")]
    public class AdminController : BaseController
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Event> _eventRepo;
        private readonly IRepository<EventOperation> _eventOperationRepo;
        private readonly IRepository<FriendOperation> _friendOperationRepo;
        private readonly IRepository<Comment> _commentRepo;
        private readonly IRepository<SearchHistory> _searchRepo;
        private readonly IRepository<Log> _logRepo;


        public AdminController(IRepository<User> userRepo,
            IRepository<Event> eventRepo,
            IRepository<EventOperation> eventOperationRepo,
            IRepository<FriendOperation> friendOperationRepo,
            IRepository<Comment> commentRepo,
            IRepository<SearchHistory> searchRepo,
            IRepository<Log> logRepo)
        {
            _userRepo = userRepo;
            _eventRepo = eventRepo;
            _eventOperationRepo = eventOperationRepo;
            _friendOperationRepo = friendOperationRepo;
            _commentRepo = commentRepo;
            _searchRepo = searchRepo;
            _logRepo = logRepo;
        }


        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            try
            {
                var users = (await _userRepo.All()).Select(u => new UserModel()
                {
                    Id = u.Id,
                    Name = u.FirstName,
                    Surname = u.LastName,
                    Email = u.Email,
                    Photo = u.Photo,
                    Role = u.Role
                }).ToList();

                if (users.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(users);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("events")]
        public async Task<IActionResult> Events()
        {
            try
            {
                var events = await _eventRepo.All();
                if (events.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(events);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("eventoperations")]
        public async Task<IActionResult> EventOperations()
        {
            try
            {
                var eo = await _eventOperationRepo.All();
                if (eo.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(eo);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("friendoperations")]
        public async Task<IActionResult> FriendOperations()
        {
            try
            {
                var fo = await _friendOperationRepo.All();
                if (fo.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(fo);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("comments")]
        public async Task<IActionResult> Comments()
        {
            try
            {
                var comments = await _commentRepo.All();
                if (comments.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(comments);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("searches")]
        public async Task<IActionResult> Searches()
        {
            try
            {
                var searches = await _searchRepo.All();
                if (searches.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(searches);
            }
            catch
            {
                return null;
            }
        }


        [HttpGet("logs")]
        public async Task<IActionResult> Logs()
        {
            try
            {
                var logs = await _logRepo.All();
                if (logs.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(logs);
            }
            catch
            {
                return null;
            }
        }

    }
}
