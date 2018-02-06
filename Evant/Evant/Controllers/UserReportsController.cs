using System;
using System.Linq;
using Evant.Contracts.DataTransferObjects.ReportType;
using Evant.Contracts.DataTransferObjects.User;
using Evant.Contracts.DataTransferObjects.UserReport;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/userreports")]
    public class UserReportsController : BaseController
    {
        private readonly IRepository<UserReport> _userReportRepo;


        public UserReportsController(IRepository<UserReport> userReportRepo)
        {
            _userReportRepo = userReportRepo;
        }


        [HttpGet("{userId}")]
        public IActionResult GetUserReports([FromRoute] Guid userId)
        {
            var userReports = _userReportRepo.Where(ur => ur.ReportedUserId == userId).Select(ur => new UserReportDetilDTO()
            {
                UserReportId = ur.Id,
                Report = new ReportTypeDTO()
                {
                    ReportTypeId = ur.ReportType.Id,
                    Name = ur.ReportType.Name,
                    Level = ur.ReportType.Level
                },
                ReporterUser = new UserInfoDTO()
                {
                    UserId = ur.ReporterUser.Id,
                    FirstName = ur.ReporterUser.FirstName,
                    LastName = ur.ReporterUser.LastName,
                    PhotoUrl = ur.ReporterUser.Photo
                },
                ReportedUser = new UserInfoDTO()
                {
                    UserId = ur.ReportedUser.Id,
                    FirstName = ur.ReportedUser.FirstName,
                    LastName = ur.ReportedUser.LastName,
                    PhotoUrl = ur.ReportedUser.Photo
                }
            }).ToList();

            if (userReports.IsNullOrEmpty())
            {
                return NotFound("Kayıt bulunamadı.");
            }
            else
            {
                return Ok(userReports);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddUserReport([FromBody] UserReportDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Eksik bilgi girdiniz.");

            Guid userId = User.GetUserId();

            var newUserReport = new UserReport()
            {
                Id = new Guid(),
                ReporterUserId = userId,
                ReportedUserId = model.ReportedUserId,
                ReportTypeId = model.ReportTypeId
            };

            var response = _userReportRepo.Insert(newUserReport);
            if (response)
            {
                return Ok("Şikayet ettiniz.");
            }
            else
            {
                return BadRequest("bir hata oluştu.");
            }
        }

    }
}
