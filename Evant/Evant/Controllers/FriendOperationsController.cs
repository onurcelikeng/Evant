using System;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/friendoperations")]
    public class FriendOperationsController : BaseController
    {
        private readonly IFriendOperationRepository _friendOperationRepo;


        public FriendOperationsController(IFriendOperationRepository friendOperationRepo)
        {
            _friendOperationRepo = friendOperationRepo;
        }


        [Authorize]
        [HttpGet]
        [Route("followers")]
        public async Task<IActionResult> GetFollowers()
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

        [Authorize]
        [HttpGet]
        [Route("followings")]
        public async Task<IActionResult> GetFollowings()
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

        [Authorize]
        [HttpGet("{friendId}")]
        public async Task<IActionResult> IsFollow([FromRoute] Guid friendId)
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

        [Authorize]
        [HttpPost("{friendId}")]
        public async Task<IActionResult> Follow([FromRoute] Guid friendId)
        {
            Guid userId = User.GetUserId();

            var selectedFriendOperation = await _friendOperationRepo.First(fo => fo.FollowerUserId == userId && fo.FollowingUserId == friendId);
            if (selectedFriendOperation != null)
                return BadRequest("Zaten takip ediyorsun.");

            var model = new FriendOperation()
            {
                Id = new Guid(),
                FollowerUserId = userId,
                FollowingUserId = friendId
            };

            var response = await _friendOperationRepo.Add(model);
            if (response)
            {
                return Ok("Takip etmeye başladın.");
            }
            else
            {
                return BadRequest("Takip ederken hata oluştu.");
            }
        }

        [Authorize]
        [HttpDelete("{friendId}")]
        public async Task<IActionResult> UnFollow([FromRoute] Guid friendId)
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

    }
}
