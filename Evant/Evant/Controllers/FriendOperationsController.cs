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
        private readonly IRepository<FriendOperation> _friendOperationRepo;
        private readonly IFriendOperationRepository _friend;


        public FriendOperationsController(IRepository<FriendOperation> friendOperationRepo,
            IFriendOperationRepository friend)
        {
            _friendOperationRepo = friendOperationRepo;
            _friend = friend;
        }


        [Authorize]
        [HttpGet]
        [Route("followers")]
        public async Task<IActionResult> GetFollowers()
        {
            Guid userId = User.GetUserId();
            var deneme = await _friend.List();
            var asd = _friendOperationRepo.GetAll();
            var followers = _friendOperationRepo.Where(fo => fo.FollowingUserId == userId).Select(u => new UserInfoDTO()
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
        public IActionResult GetFollowings()
        {
            Guid userId = User.GetUserId();
            var followings = _friendOperationRepo.Where(fo => fo.FollowerUserId == userId).Select(u => new UserInfoDTO()
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
        public IActionResult IsFollow([FromRoute] Guid friendId)
        {
            Guid userId = User.GetUserId();

            var selectedFriendOperation = _friendOperationRepo.First(fo => fo.FollowingUserId == friendId && fo.FollowerUserId == userId);
            if(selectedFriendOperation == null)
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
        public IActionResult Follow([FromRoute] Guid friendId)
        {
            Guid userId = User.GetUserId();

            var selectedFriendOperation = _friendOperationRepo.First(fo => fo.FollowerUserId == userId && fo.FollowingUserId == friendId);
            if (selectedFriendOperation != null)
                return BadRequest("Zaten takip ediyorsun.");

            var model = new FriendOperation()
            {
                Id = new Guid(),
                FollowerUserId = userId,
                FollowingUserId = friendId
            };

            var response = _friendOperationRepo.Insert(model);
            if(response)
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
        public IActionResult UnFollow([FromRoute] Guid friendId)
        {
            Guid userId = User.GetUserId();

            var selectedFriendOperation = _friendOperationRepo.First(fo => fo.FollowerUserId == userId && fo.FollowingUserId == friendId);
            if (selectedFriendOperation == null)
                return BadRequest("Zaten takip etmiyorsun.");

            var response = _friendOperationRepo.Delete(selectedFriendOperation);
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
