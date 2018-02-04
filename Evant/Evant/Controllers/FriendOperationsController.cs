using System;
using System.Linq;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
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


        public FriendOperationsController(IRepository<FriendOperation> friendOperationRepo)
        {
            _friendOperationRepo = friendOperationRepo;
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetFollowers()
        {
            Guid userId = User.GetUserId();
            var users = _friendOperationRepo.Where(fo => fo.FollowingId == userId).Select(u => new UserInfoDTO()
            {
                UserId = u.FollowerUser.Id,
                FirstName = u.FollowerUser.FirstName,
                LastName = u.FollowerUser.LastName,
                PhotoUrl = u.FollowerUser.Photo
            });

            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetFollowings()
        {
            Guid userId = User.GetUserId();
            var users = _friendOperationRepo.Where(fo => fo.FollowerId == userId).Select(u => new UserInfoDTO()
            {
                UserId = u.FollowingUser.Id,
                FirstName = u.FollowingUser.FirstName,
                LastName = u.FollowingUser.LastName,
                PhotoUrl = u.FollowingUser.Photo
            });

            return Ok(users);
        }

        [Authorize]
        [HttpPost("{friendId}")]
        public IActionResult Follow([FromRoute] Guid friendId)
        {
            Guid userId = User.GetUserId();

            var selectedFriendOperation = _friendOperationRepo.First(fo => fo.FollowerId == userId && fo.FollowingId == friendId);
            if (selectedFriendOperation != null)
                return BadRequest("Zaten takip ediyorsunuz.");

            var model = new FriendOperation()
            {
                Id = new Guid(),
                FollowerId = userId,
                FollowingId = friendId
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

            var selectedFriendOperation = _friendOperationRepo.First(fo => fo.FollowerId == userId && fo.FollowingId == friendId);
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
