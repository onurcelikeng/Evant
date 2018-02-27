using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Timeline;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IFriendOperationRepository _friendOperationRepo;
        private readonly IEventOperationRepository _eventOperationRepo;
        private readonly ISearchHelper _searchHelper;
        private readonly ILogHelper _logHelper;


        public UsersController(IUserRepository userRepo,
            IEventRepository eventRepo,
            ICommentRepository commentRepo,
            IFriendOperationRepository friendOperationRepo,
            IEventOperationRepository eventOperationRepo,
            ISearchHelper searchHelper,
            ILogHelper logHelper)
        {
            _userRepo = userRepo;
            _eventRepo = eventRepo;
            _commentRepo = commentRepo;
            _friendOperationRepo = friendOperationRepo;
            _eventOperationRepo = eventOperationRepo;
            _searchHelper = searchHelper;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            try
            {
                var user = await _userRepo.GetUser(userId);
                if (user == null)
                {
                    return BadRequest("Kayıt bulunamadı.");
                }
                else
                {
                    var model = new BaseUserDetailDTO()
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhotoUrl = user.Photo,
                        FollowersCount = user.Followers.Count,
                        FollowingsCount = user.Followings.Count
                    };

                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "GetUser", ex.Message);
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = (await _userRepo.All()).Select(u => new UserInfoDTO()
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhotoUrl = u.Photo
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
        [HttpGet("timeline/{userId}")]
        public async Task<IActionResult> UserTimeline([FromRoute] Guid userId)
        {
            List<TimelineDTO> timeline = new List<TimelineDTO>();

            var eventsTimeline = (await _eventRepo.UserEvents(userId)).Select(e => new TimelineDTO()
            {
                Header = "Create Event",
                Body = e.Title,
                Image = e.Photo,
                CreateAt = e.CreatedAt,
                CustomId = e.Id,
                Type = "create-event"
            }).ToList();
            if (!eventsTimeline.IsNullOrEmpty())
            {
                timeline.AddRange(eventsTimeline);
            }

            var eventOperationsTimeline = (await _eventOperationRepo.UserEventOperations(userId)).Select(eo => new TimelineDTO()
            {
                Header = "Join Event",
                Body = eo.Event.Title,
                Image = eo.Event.Photo,
                CreateAt = eo.CreatedAt,
                CustomId = eo.Id,
                Type = "join-event"
            }).ToList();
            if (!eventOperationsTimeline.IsNullOrEmpty())
            {
                timeline.AddRange(eventOperationsTimeline);
            }

            var friendOperationsTimeline = (await _friendOperationRepo.Followings(userId)).Select(fo => new TimelineDTO()
            {
                Header = "Follow Friend",
                Body = fo.FollowingUser.FirstName + " " + fo.FollowingUser.LastName,
                Image = fo.FollowingUser.Photo,
                CreateAt = fo.CreatedAt,
                CustomId = fo.FollowingUserId,
                Type = "follow-friend"
            }).ToList();
            if (!friendOperationsTimeline.IsNullOrEmpty())
            {
                timeline.AddRange(friendOperationsTimeline);
            }

            if (timeline.IsNullOrEmpty())
            {
                return NotFound("Kayıt bulunamadı.");
            }
            else
            {
                return Ok(timeline);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("search/{query}")]
        public async Task<IActionResult> SearchUsers(string query)
        {
            try
            {
                Guid userId = User.GetUserId();
                await _searchHelper.Add(userId, query);

                var users = (await _userRepo.Search(query)).Select(u => new UserInfoDTO()
                {
                    UserId = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhotoUrl = u.Photo
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
                _logHelper.Log("Users", 500, "SearcUsers", ex.Message);
                return null;
            }
        }

    }
}
