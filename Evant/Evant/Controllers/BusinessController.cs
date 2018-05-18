using System;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Business;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/business")]
    public class BusinessController : BaseController
    {
        private readonly IRepository<Business> _businessRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogHelper _logHelper;


        public BusinessController(IRepository<Business> businessRepo,
            IUserRepository userRepo,
            ILogHelper logHelper)
        {
            _businessRepo = businessRepo;
            _userRepo = userRepo;
            _logHelper = logHelper;
        }


        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> SwitchtoNormal()
        {
            try
            {
                Guid userId = User.GetUserId();

                var user = await _userRepo.First(u => u.Id == userId);
                if (user == null)
                    return BadRequest("Kayıt bulunamadı.");

                user.IsBusinessAccount = false;
                user.UpdateAt = DateTime.Now;

                var response = await _userRepo.Update(user);
                if (response)
                {
                    return Ok("Normal hesaba geçildi.");
                }
                else
                {
                    return BadRequest("Normal hesaba geçilemedi.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("BusinessController", 500, "SwitchtoNormal", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SwitchToBusiness([FromBody]BusinessDTO model)
        {
            try
            {
                Guid userId = User.GetUserId();
                var user = await _userRepo.First(u => u.Id == userId);
                if (user == null)
                    return BadRequest("Kayıt bulunamadı.");

                user.IsBusinessAccount = true;
                user.UpdateAt = DateTime.Now;

                var userResponse = await _userRepo.Update(user);
                if (!userResponse)
                    return BadRequest("Bir hata oluştu.");

                var business = await _businessRepo.First(s => s.UserId == userId);
                if (business == null)
                    return NotFound("Kayıt bulunamadı.");

                business.BusinessType = model.BusinessType;
                business.UpdateAt = DateTime.Now;      
                if (model.BusinessType == "Free")
                {
                    business.IsAgeAnalysis = true;
                    business.IsSendNotificationUsers = false;
                    business.IsCommentAnalysis = false;
                    business.IsAttendedUserAnalysis = false;
                    business.IsChatBotSupport = false;
                    business.ExpireDate = DateTime.Now.AddYears(10);
                }
                else if (model.BusinessType == "Basic")
                {
                    business.IsAgeAnalysis = true;
                    business.IsSendNotificationUsers = true;
                    business.IsCommentAnalysis = false;
                    business.IsAttendedUserAnalysis = false;
                    business.IsChatBotSupport = false;
                    business.ExpireDate = DateTime.Now.AddYears(1);
                }
                else if (model.BusinessType == "Gold")
                {
                    business.IsAgeAnalysis = true;
                    business.IsSendNotificationUsers = true;
                    business.IsCommentAnalysis = true;
                    business.IsAttendedUserAnalysis = true;
                    business.IsChatBotSupport = false;
                    business.ExpireDate = DateTime.Now.AddYears(1);
                }
                else if (model.BusinessType == "Platinum")
                {
                    business.IsAgeAnalysis = true;
                    business.IsSendNotificationUsers = true;
                    business.IsCommentAnalysis = true;
                    business.IsAttendedUserAnalysis = true;
                    business.IsChatBotSupport = true;
                    business.ExpireDate = DateTime.Now.AddYears(1);
                }

                var result = await _businessRepo.Update(business);
                if (result)
                {
                    return Ok(model.BusinessType + " Business hesabına geçiş yaptınız.");
                }
                else
                {
                    return BadRequest(model.BusinessType + " Business hesabına geçiş yapamadınız.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("BusinessController", 500, "SwitchToBusiness", ex.Message);
                return null;
            }
        }

    }
}
