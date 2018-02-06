using System;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/logs")]
    public class LogsController : BaseController
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

            return Ok(logs);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLog([FromRoute] Guid id)
        {
            var log = _logRepo.First(l => l.Id == id);
            if (log == null)
            {
                return NotFound("Böyle bir log yok.");
            }

            var response = _logRepo.Delete(log);
            if (response)
            {
                return Ok("Log silindi.");
            }
            else
            {
                return BadRequest("Log silinemedi.");
            }
        }

    }
}
