using System;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.UserSettingDTO.cs;
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
                if(settings == null)
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    var model = new UserSettingDTO()
                    {
                        UserSettingId = settings.Id,
                        Theme = settings.Theme,
                        Language = settings.Language,
                        IsCommentNotif = settings.IsCommentNotif,
                        IsEventNewComerNotif = settings.IsEventNewComerNotif,
                        IsEventUpdateNotif = settings.IsEventUpdateNotif,
                        IsFriendshipNotif = settings.IsFriendshipNotif
                    };

                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "Register", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ChangeUserSetting([FromBody] UserSettingDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Eksik bilgi girdiniz.");
            }

            Guid userId = User.GetUserId();
            var selectedUserSetting = await _userSettingRepo.First(us => us.UserId == userId);
            if (selectedUserSetting == null)
            {
                return NotFound("Kayıt bulunamadı.");
            }
            else
            {
                selectedUserSetting.UpdateAt = DateTime.Now;
                selectedUserSetting.Theme = model.Theme;
                selectedUserSetting.Language = model.Language;
                selectedUserSetting.IsCommentNotif = model.IsCommentNotif;
                selectedUserSetting.IsEventNewComerNotif = model.IsEventNewComerNotif;
                selectedUserSetting.IsEventUpdateNotif = model.IsEventUpdateNotif;
                selectedUserSetting.IsFriendshipNotif = model.IsFriendshipNotif;

                var response = await _userSettingRepo.Update(selectedUserSetting);
                if (response)
                {
                    return Ok("Kullanıcı ayarları güncellendi.");
                }
                else
                {
                    return BadRequest("Kullanıcı ayarları güncellenemedi.");
                }
            }
        }

    }
}
