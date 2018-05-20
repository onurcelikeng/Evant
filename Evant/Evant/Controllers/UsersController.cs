using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Timeline;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IFriendOperationRepository _friendOperationRepo;
        private readonly IEventOperationRepository _eventOperationRepo;
        private readonly IRepository<UserSetting> _userSettingRepo;
        private readonly ISearchHelper _searchHelper;
        private readonly ILogHelper _logHelper;


        public UsersController(IUserRepository userRepo,
            IEventRepository eventRepo,
            ICommentRepository commentRepo,
            IFriendOperationRepository friendOperationRepo,
            IEventOperationRepository eventOperationRepo,
            IRepository<UserSetting> userSettingRepo,
            ISearchHelper searchHelper,
            ILogHelper logHelper)
        {
            _userRepo = userRepo;
            _eventRepo = eventRepo;
            _commentRepo = commentRepo;
            _friendOperationRepo = friendOperationRepo;
            _eventOperationRepo = eventOperationRepo;
            _userSettingRepo = userSettingRepo;
            _searchHelper = searchHelper;
            _logHelper = logHelper;
        }


        [HttpGet()]
        [Route("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            try
            {
                var user = await _userRepo.GetUser(userId);
                if (user == null)
                    return BadRequest("Kayıt bulunamadı.");

                return Ok(new BaseUserDetailDTO()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhotoUrl = user.Photo,
                    FollowersCount = user.Followers.Count,
                    FollowingsCount = user.Followings.Count
                });
            }
            catch (Exception ex)
            {
                _logHelper.Log("UsersController", 500, "GetUser", ex.Message);
                return null;
            }
        }

        [HttpGet()]
        [Route("timeline/{userId}")]
        public async Task<IActionResult> UserTimeline([FromRoute] Guid userId)
        {
            try
            {
                var timeline = new List<TimelineDTO>();

                var userSetting = await _userSettingRepo.First(us => us.UserId == userId);
                if (userSetting == null)
                    return NotFound("Kullanıcı ayarı bulunamadı.");

                //User Events
                if (userSetting.IsCreateEventVisibleTimeline)
                {
                    var eventsTimeline = (await _eventRepo.UserEvents(userId)).Select(e => new TimelineDTO()
                    {
                        Header = e.Title,
                        Body = TimelineHelper.GenerateUserCreateEventBody(e),
                        Image = e.Photo,
                        CreateAt = e.CreatedAt,
                        CustomId = e.Id,
                        Type = "create-event",
                        LineColor = "#a6714e"
                    }).ToList();
                    if (!eventsTimeline.IsNullOrEmpty())
                    {
                        timeline.AddRange(eventsTimeline);
                    }
                }

                //Event Operations
                if (userSetting.IsJoinEventVisibleTimeline)
                {
                    var eventOperationsTimeline = (await _eventOperationRepo.UserEventOperations(userId)).Select(eo => new TimelineDTO()
                    {
                        Header = eo.Event.Title,
                        Body = TimelineHelper.GenerateUserJoinEventBody(eo.Event),
                        Image = eo.Event.Photo,
                        CreateAt = eo.CreatedAt,
                        CustomId = eo.EventId,
                        Type = "join-event",
                        LineColor = "#f78f8f"
                    }).ToList();
                    if (!eventOperationsTimeline.IsNullOrEmpty())
                    {
                        timeline.AddRange(eventOperationsTimeline);
                    }
                }

                //Following Friend Operations
                if (userSetting.IsFollowingVisibleTimeline)
                {
                    var followFriendOperationsTimeline = (await _friendOperationRepo.Followings(userId)).Select(fo => new TimelineDTO()
                    {
                        Header = fo.FollowingUser.FirstName + " " + fo.FollowingUser.LastName,
                        Body = TimelineHelper.GenerateUserFollowingBody(),
                        Image = null,
                        CreateAt = fo.CreatedAt,
                        CustomId = fo.FollowingUserId,
                        Type = "following",
                        LineColor = "#ffeea3"
                    }).ToList();
                    if (!followFriendOperationsTimeline.IsNullOrEmpty())
                    {
                        timeline.AddRange(followFriendOperationsTimeline);
                    }
                }

                //Follow Friend Operations
                if (userSetting.IsFollowerVisibleTimeline)
                {
                    var followingFriendOperationsTimeline = (await _friendOperationRepo.Followers(userId)).Select(fo => new TimelineDTO()
                    {
                        Header = fo.FollowerUser.FirstName + " " + fo.FollowerUser.LastName,
                        Body = TimelineHelper.GenerateUserFollowerBody(),
                        Image = null,
                        CreateAt = fo.CreatedAt,
                        CustomId = fo.FollowerUserId,
                        Type = "follower",
                        LineColor = "#ffeea3"
                    }).ToList();
                    if (!followingFriendOperationsTimeline.IsNullOrEmpty())
                    {
                        timeline.AddRange(followingFriendOperationsTimeline);
                    }
                }

                //Comments
                if (userSetting.IsCommentVisibleTimeline)
                {
                    var commentsTimeline = (await _commentRepo.UserComments(userId)).Select(c => new TimelineDTO()
                    {
                        Header = c.Event.Title,
                        Body = TimelineHelper.GenerateUserCommentBody(userId, c),
                        Image = null,
                        CreateAt = c.CreatedAt,
                        CustomId = c.EventId,
                        Type = "comment-event",
                        LineColor = "#dcd5f2"
                    }).ToList();
                    if (!commentsTimeline.IsNullOrEmpty())
                    {
                        timeline.AddRange(commentsTimeline);
                    }
                }

                if (timeline.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(timeline.OrderByDescending(t => t.CreateAt));
            }
            catch (Exception ex)
            {
                _logHelper.Log("UsersController", 500, "UserTimeline", ex.Message);
                return null;
            }
        }

        [HttpGet]
        [Route("search/{query}")]
        public async Task<IActionResult> SearchUsers(string query)
        {
            try
            {
                var users = (await _userRepo.Search(query)).Select(row => new UserInfoDTO()
                {
                    UserId = row.Id,
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    PhotoUrl = row.Photo
                }).ToList();

                if (users.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logHelper.Log("UsersController", 500, "SearcUsers", ex.Message);
                return null;
            }
        }

    }
}
