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
    public class ReportTypesController : Controller
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
                Id = c.Id,
                Name = c.Name,
                Level = c.Level
            });

            return BaseController.Instance.Result(reportTypes, 200);
        }

        [HttpGet("{id}")]
        public IActionResult GetReportType([FromRoute] Guid id)
        {
            var reportType = _reportTypeRepo.First(c => c.Id == id);
            if (reportType == null)
            {
                return BaseController.Instance.Result(null, 404, "Böyle bir şikayet türü yok.");
            }

            return BaseController.Instance.Result(reportType, 200);
        }

        [HttpPut]
        public IActionResult PutReportType([FromBody] ReportTypeDTO reportType)
        {
            if (!ModelState.IsValid)
            {
                return BaseController.Instance.Result(null, 400);
            }

            var selectedReportType = _reportTypeRepo.First(c => c.Id == reportType.Id);
            if (selectedReportType == null)
            {
                return BaseController.Instance.Result(null, 404, "Şikayet türü bulunamadı.");
            }
            else
            {
                selectedReportType.Name = reportType.Name;
                selectedReportType.Level = reportType.Level;
                selectedReportType.UpdateAt = DateTime.Now;

                var response = _reportTypeRepo.Update(selectedReportType);
                if (response)
                {
                    return BaseController.Instance.Result(null, 200, "Şikayet türü güncellendi.");
                }
                else
                {
                    return BaseController.Instance.Result(null, 500, "Şikayet türü güncellenemedi.");
                }
            }
        }

        [HttpPost]
        public IActionResult PostReportType([FromBody] ReportTypeDTO reportType)
        {
            if (!ModelState.IsValid)
            {
                return BaseController.Instance.Result(null, 400);
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
                return BaseController.Instance.Result(null, 200, "Şikayet türü eklendi.");
            }
            else
            {
                return BaseController.Instance.Result(null, 500, "Şikayet türü eklenemedi.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReportType([FromRoute] Guid id)
        {
            var reportType = _reportTypeRepo.First(c => c.Id == id);
            if (reportType == null)
            {
                return BaseController.Instance.Result(null, 404, "Böyle bir Şikayet türü yok.");
            }

            var response = _reportTypeRepo.Delete(reportType);
            if (response)
            {
                return BaseController.Instance.Result(null, 200, "Şikayet türü silindi.");
            }
            else
            {
                return BaseController.Instance.Result(null, 500, "Şikayet türü silinemedi.");
            }
        }

    }
}