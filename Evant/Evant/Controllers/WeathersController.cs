using System;
using System.Threading.Tasks;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/weathers")]
    public class WeathersController : BaseController
    {
        private readonly IRepository<Weather> _weatherRepo;


        public WeathersController(IRepository<Weather> weatherRepo)
        {
            _weatherRepo = weatherRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetWeather()
        {
            try
            {
                var weather = await _weatherRepo.First(w => w.Id.ToString() == "d2bf948b-969f-4640-1986-08d5c1b86a5d");
                if(weather == null)
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    return Ok(weather);
                }
            }
            catch
            {
                return null;
            }
        }

        [HttpPut("{type}")]
        public async Task<IActionResult> UpdateWeather([FromRoute] string type)
        {
            var weather = await _weatherRepo.First(w => w.Id.ToString() == "d2bf948b-969f-4640-1986-08d5c1b86a5d");
            if (weather == null)
            {
                return NotFound("Kayıt bulunamadı.");
            }
            else
            {
                if(type == "1") weather.Status = "01";
                else if (type == "2") weather.Status = "03";
                else if (type == "3") weather.Status = "06";
                else if (type == "4") weather.Status = "12";

                weather.UpdateAt = DateTime.UtcNow;
                var response = await _weatherRepo.Update(weather);
                if (response)
                    return Ok("Güncellendi.");

                else
                    return BadRequest("Güncellenemedi.");
            }

        }

    }
}