using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ILogHelper _logHelper;


        public UsersController(IUserRepository userRepo,
            ILogHelper logHelper)
        {
            _userRepo = userRepo;
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
            });

            return Ok(users);
        }

    }
}
