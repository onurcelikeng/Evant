using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LogsController : Controller
    {
        private readonly IRepository<Log> _logRepo;


        public LogsController(IRepository<Log> logRepo)
        {
            _logRepo = logRepo;
        }


        [HttpGet]
        public IActionResult GetLogs()
        {
            var logs = _logRepo.GetAll();

            return BaseController.Instance.Result(logs, 200);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLog([FromRoute] Guid id)
        {
            var log = _logRepo.First(l => l.Id == id);
            if (log == null)
            {
                return BaseController.Instance.Result(null, 404, "Böyle bir log yok.");
            }

            var response = _logRepo.Delete(log);
            if (response)
            {
                return BaseController.Instance.Result(null, 200, "Log silindi.");
            }
            else
            {
                return BaseController.Instance.Result(null, 500, "Log silinemedi.");
            }
        }
    }
}