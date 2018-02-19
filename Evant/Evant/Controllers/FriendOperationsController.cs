using System;
using System.Linq;
using System.Threading.Tasks;
using Evant.Constants;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Evant.NotificationCenter.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Evant.Constants.NotificationConstant;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/friendoperations")]
    public class FriendOperationsController : BaseController
    {
        private readonly IFriendOperationRepository _friendOperationRepo;
        private readonly INotificationHelper _notificationHelper;
        private readonly ILogHelper _logHelper;


        public FriendOperationsController(IFriendOperationRepository friendOperationRepo,
            INotificationHelper notificationHelper,
            ILogHelper logHelper)
        {
            _friendOperationRepo = friendOperationRepo;
            _notificationHelper = notificationHelper;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpGet]
        [Route("followers")]
        public async Task<IActionResult> GetFollowers()
        {
            try
            {
                Guid userId = User.GetUserId();

                var followers = (await _friendOperationRepo.Followers(userId)).Select(u => new UserInfoDTO()
                {
                    UserId = u.FollowerUser.Id,
                    FirstName = u.FollowerUser.FirstName,
                    LastName = u.FollowerUser.LastName,
                    PhotoUrl = u.FollowerUser.Photo
                }).ToList();

                if (followers.IsNullOrEmpty())
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    return Ok(followers);
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Logs", 500, "GetFollowers", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("followings")]
        public async Task<IActionResult> GetFollowings()
        {
            try
            {
                Guid userId = User.GetUserId();
                var followings = (await _friendOperationRepo.Followings(userId)).Select(u => new UserInfoDTO()
                {
                    UserId = u.FollowingUser.Id,
                    FirstName = u.FollowingUser.FirstName,
                    LastName = u.FollowingUser.LastName,
                    PhotoUrl = u.FollowingUser.Photo
                }).ToList();

                if (followings.IsNullOrEmpty())
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    return Ok(followings);
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Logs", 500, "GetFollowings", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet("{friendId}")]
        public async Task<IActionResult> IsFollow([FromRoute] Guid friendId)
        {
            try
            {
                Guid userId = User.GetUserId();

                var selectedFriendOperation = await _friendOperationRepo.First(fo => fo.FollowingUserId == friendId && fo.FollowerUserId == userId);
                if (selectedFriendOperation == null)
                {
                    return BadRequest("Takip etmiyorsun.");
                }
                else
                {
                    return Ok("Takip ediyorsun.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Logs", 500, "IsFollow", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost("{friendId}")]
        public async Task<IActionResult> Follow([FromRoute] Guid friendId)
        {
            try
            {
                Guid userId = User.GetUserId();

                var friendOperation = await _friendOperationRepo.First(fo => fo.FollowerUserId == userId && fo.FollowingUserId == friendId);
                if (friendOperation != null)
                {
                    return BadRequest("Zaten takip ediyorsun.");
                }
                else
                {
                    var entity = new FriendOperation()
                    {
                        Id = new Guid(),
                        FollowerUserId = userId,
                        FollowingUserId = friendId
                    };

                    var response = await _friendOperationRepo.Add(entity);
                    if (response)
                    {
                        await _notificationHelper.SendFollowNotification(userId, friendId);
                        return Ok("Takip etmeye başladın.");
                    }
                    else
                    {
                        return BadRequest("Takip ederken hata oluştu.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Logs", 500, "Follow", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpDelete("{friendId}")]
        public async Task<IActionResult> UnFollow([FromRoute] Guid friendId)
        {
            try
            {
                Guid userId = User.GetUserId();

                var selectedFriendOperation = await _friendOperationRepo.First(fo => fo.FollowerUserId == userId && fo.FollowingUserId == friendId);
                if (selectedFriendOperation == null)
                {
                    return BadRequest("Zaten takip etmiyorsun.");
                }

                var response = await _friendOperationRepo.Delete(selectedFriendOperation);
                if (response)
                {
                    return Ok("Takip etmeyi bıraktın.");
                }
                else
                {
                    return BadRequest("Takip etmeyi bırakırken hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Logs", 500, "UnFollow", ex.Message);
                return null;
            }
        }

    }
}
