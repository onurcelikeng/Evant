using System;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.UserSettingDTO;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/settings")]
    public class UserSettingsController : BaseController
    {
        private readonly IRepository<UserSetting> _userSettingRepo;
        private readonly ILogHelper _logHelper;


        public UserSettingsController(IRepository<UserSetting> userSettingRepo,
            ILogHelper logHelper)
        {
            _userSettingRepo = userSettingRepo;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserSettings()
        {
            try
            {
                Guid userId = User.GetUserId();

                var settings = await _userSettingRepo.First(s => s.UserId == userId);
                if (settings == null)
                    return NotFound("Kayıt bulunamadı.");

                var model = new UserSettingDTO()
                {
                    UserSettingId = settings.Id,
                    Theme = settings.Theme,
                    Language = settings.Language,
                    IsCommentNotif = settings.IsCommentNotif,
                    IsEventNewComerNotif = settings.IsEventNewComerNotif,
                    IsEventUpdateNotif = settings.IsEventUpdateNotif,
                    IsFriendshipNotif = settings.IsFriendshipNotif,
                    IsCommentVisibleTimeline = settings.IsCommentVisibleTimeline,
                    IsJoinEventVisibleTimeline = settings.IsJoinEventVisibleTimeline,
                    IsFollowerVisibleTimeline = settings.IsFollowerVisibleTimeline,
                    IsFollowingVisibleTimeline = settings.IsFollowingVisibleTimeline
                };
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logHelper.Log("UserSettingsController", 500, "GetUserSettings", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ChangeUserSetting([FromBody] UserSettingDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                Guid userId = User.GetUserId();

                var selectedUserSetting = await _userSettingRepo.First(us => us.UserId == userId);
                if (selectedUserSetting == null)
                    return NotFound("Kayıt bulunamadı.");

                selectedUserSetting.UpdateAt = DateTime.Now;
                selectedUserSetting.Theme = model.Theme;
                selectedUserSetting.Language = model.Language;
                selectedUserSetting.IsCommentNotif = model.IsCommentNotif;
                selectedUserSetting.IsEventNewComerNotif = model.IsEventNewComerNotif;
                selectedUserSetting.IsEventUpdateNotif = model.IsEventUpdateNotif;
                selectedUserSetting.IsFriendshipNotif = model.IsFriendshipNotif;
                selectedUserSetting.IsCommentVisibleTimeline = model.IsCommentVisibleTimeline;
                selectedUserSetting.IsJoinEventVisibleTimeline = model.IsJoinEventVisibleTimeline;
                selectedUserSetting.IsFollowingVisibleTimeline = model.IsFollowingVisibleTimeline;
                selectedUserSetting.IsFollowerVisibleTimeline = model.IsFollowerVisibleTimeline;

                var response = await _userSettingRepo.Update(selectedUserSetting);
                if (response)
                {
                    return Ok("Ayarlarınızı güncellendiniz.");
                }
                else
                {
                    return BadRequest("Ayarlarınızı güncelleyemediniz.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("UserSettingsController", 500, "ChangeUserSetting", ex.Message);
                return null;
            }
        }

    }
}
