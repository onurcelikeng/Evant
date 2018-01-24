using System;
using System.Linq;
using Evant.Contracts.DataTransferObjects.ReportType;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ReportTypesController : BaseController
    {
        private readonly IRepository<ReportType> _reportTypeRepo;


        public ReportTypesController(IRepository<ReportType> reportTypeRepo)
        {
            _reportTypeRepo = reportTypeRepo;
        }


        [HttpGet]
        public IActionResult GetReportTypes()
        {
            var reportTypes = _reportTypeRepo.GetAll().Select(c => new ReportTypeDTO()
            {
                ReportTypeId = c.Id,
                Name = c.Name,
                Level = c.Level
            });

            return Ok(reportTypes);
        }

        [HttpGet("{id}")]
        public IActionResult GetReportType([FromRoute] Guid id)
        {
            var reportType = _reportTypeRepo.First(c => c.Id == id);
            if (reportType == null)
            {
                return NotFound("Böyle bir şikayet türü yok.");
            }

            return Ok(reportType);
        }

        [HttpPut]
        public IActionResult PutReportType([FromBody] ReportTypeDTO reportType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(null);
            }

            var selectedReportType = _reportTypeRepo.First(c => c.Id == reportType.ReportTypeId);
            if (selectedReportType == null)
            {
                return NotFound("Şikayet türü bulunamadı.");
            }
            else
            {
                selectedReportType.Name = reportType.Name;
                selectedReportType.Level = reportType.Level;
                selectedReportType.UpdateAt = DateTime.Now;

                var response = _reportTypeRepo.Update(selectedReportType);
                if (response)
                {
                    return Ok("Şikayet türü güncellendi.");
                }
                else
                {
                    return BadRequest("Şikayet türü güncellenemedi.");
                }
            }
        }

        [HttpPost]
        public IActionResult PostReportType([FromBody] ReportTypeDTO reportType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(null);
            }

            var response = _reportTypeRepo.Insert(new ReportType()
            {
                Id = new Guid(),
                Name = reportType.Name,
                Level = reportType.Level,
                CreatedAt = DateTime.Now
            });
            if (response)
            {
                return Ok("Şikayet türü eklendi.");
            }
            else
            {
                return BadRequest("Şikayet türü eklenemedi.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReportType([FromRoute] Guid id)
        {
            var reportType = _reportTypeRepo.First(c => c.Id == id);
            if (reportType == null)
            {
                return NotFound("Böyle bir Şikayet türü yok.");
            }

            var response = _reportTypeRepo.Delete(reportType);
            if (response)
            {
                return Ok("Şikayet türü silindi.");
            }
            else
            {
                return BadRequest("Şikayet türü silinemedi.");
            }
        }
    }
}
