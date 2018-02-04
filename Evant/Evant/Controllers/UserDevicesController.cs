using System;
using Evant.Contracts.DataTransferObjects.UserDevice;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/devices")]
    public class UserDevicesController : BaseController
    {
        private readonly IRepository<UserDevice> _userDevicesRepo;


        public UserDevicesController(IRepository<UserDevice> userDevicesRepo)
        {
            _userDevicesRepo = userDevicesRepo;
        }


        [Authorize]
        [HttpPost]
        public IActionResult SaveDevice([FromBody] UserDeviceDTO device)
        {
            if (!ModelState.IsValid)
                return BadRequest("Eksik bilgi girdiniz.");

            var selectedDevice = _userDevicesRepo.First(d => d.DeviceId == device.DeviceId);
            if (selectedDevice != null)
            {
                selectedDevice.IsLoggedin = true;
                selectedDevice.UpdateAt = DateTime.Now;
                selectedDevice.PlayerId = device.PlayerId;

                var response = _userDevicesRepo.Update(selectedDevice);
                if (response)
                    return Ok("güncellendi.");
                else
                    return BadRequest("güncellenemedi.");
            }
            else
            {
                Guid userId = User.GetUserId();
                var newDevice = new UserDevice()
                {
                    Id = new Guid(),
                    UserId = userId,
                    DeviceId = device.DeviceId,
                    PlayerId = device.PlayerId,
                    Brand = device.Brand,
                    Model = device.Model,
                    OS = device.OS,
                    IsLoggedin = true,
                };

                var response = _userDevicesRepo.Insert(newDevice);
                if (response)
                    return Ok("eklendi.");
                else
                    return BadRequest("eklenemedi.");
            }
        }

        [Authorize]
        [HttpGet("{deviceId}")]
        public IActionResult CloseDevice([FromRoute] string deviceId)
        {
            var selectedDevice = _userDevicesRepo.First(d => d.DeviceId == deviceId);
            if (selectedDevice == null)
                return BadRequest("cihaz bulunamadı.");
            else
            {
                selectedDevice.IsLoggedin = false;
                selectedDevice.UpdateAt = DateTime.Now;

                var response = _userDevicesRepo.Update(selectedDevice);
                if (response)
                    return Ok("cihaz kapatıldı.");
                else
                    return BadRequest("cihaz kapatılamadı.");
            }
        }

    }
}
