using System;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.Business;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Evant.Pay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Evant.Constants.BusinessConstant;

namespace Evant.Controllers
{
    [Authorize]
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
                user.UpdateAt = DateTime.UtcNow;

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

        [HttpPost]
        public async Task<IActionResult> SwitchToBusiness([FromBody]BusinessDTO model)
        {
            try
            {
                if (model.Payment != null)
                {
                    Iyzico iyzico = new Iyzico();
                    var iyzicoResponse = await iyzico.Operation(model.Payment);
                    if (!iyzicoResponse)
                        return BadRequest("Ödeme işleminde bir hata oluştu.");
                }

                Guid userId = User.GetUserId();
                var user = await _userRepo.First(u => u.Id == userId);
                if (user == null)
                    return BadRequest("Kayıt bulunamadı.");

                user.IsBusinessAccount = true;
                user.UpdateAt = DateTime.UtcNow;

                var userResponse = await _userRepo.Update(user);
                if (!userResponse)
                    return BadRequest("Bir hata oluştu.");

                var business = await _businessRepo.First(s => s.UserId == userId);
                if (business == null)
                    return NotFound("Kayıt bulunamadı.");

                business.BusinessType = model.BusinessType;
                business.UpdateAt = DateTime.UtcNow;

                if (model.BusinessType == BusinessType.Free.ToString())
                {
                    business.IsAgeAnalysis = true;
                    business.IsSendNotificationUsers = false;
                    business.IsCommentAnalysis = false;
                    business.IsAttendedUserAnalysis = false;
                    business.IsChatBotSupport = false;
                    business.ExpireDate = DateTime.UtcNow.AddYears(10);
                }
                else if (model.BusinessType == BusinessType.Basic.ToString())
                {
                    business.IsAgeAnalysis = true;
                    business.IsSendNotificationUsers = true;
                    business.IsCommentAnalysis = false;
                    business.IsAttendedUserAnalysis = false;
                    business.IsChatBotSupport = false;
                    business.ExpireDate = DateTime.UtcNow.AddYears(1);
                }
                else if (model.BusinessType == BusinessType.Gold.ToString())
                {
                    business.IsAgeAnalysis = true;
                    business.IsSendNotificationUsers = true;
                    business.IsCommentAnalysis = true;
                    business.IsAttendedUserAnalysis = true;
                    business.IsChatBotSupport = false;
                    business.ExpireDate = DateTime.UtcNow.AddYears(1);
                }
                else if (model.BusinessType == BusinessType.Platinum.ToString())
                {
                    business.IsAgeAnalysis = true;
                    business.IsSendNotificationUsers = true;
                    business.IsCommentAnalysis = true;
                    business.IsAttendedUserAnalysis = true;
                    business.IsChatBotSupport = true;
                    business.ExpireDate = DateTime.UtcNow.AddYears(1);
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

        [HttpGet]
        public async Task<IActionResult> IsExistBusinessAccount()
        {
            try
            {
                Guid userId = User.GetUserId();
                var business = await _businessRepo.First(s => s.UserId == userId);
                if (business == null)
                    return NotFound("Kayıt bulunamadı.");

                return Ok(new BusinessDetailDTO()
                {
                    BusinessType = business.BusinessType,
                    ExpireAt = business.ExpireDate
                });
            }
            catch (Exception ex)
            {
                _logHelper.Log("BusinessController", 500, "IsExistBusinessAccount", ex.Message);
                return null;
            }
        }

    }
}
