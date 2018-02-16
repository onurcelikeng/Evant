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
                {
                    return Ok("Cihaz güncellendi.");
                }
                else
                {
                    return BadRequest("Cihaz güncellenemedi.");
                }
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
                {
                    return Ok("Cihaz güncellendi.");
                }
                else
                {
                    return BadRequest("Cihaz güncellenemedi.");
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("logout/{deviceId}")]
        public IActionResult CloseDevice([FromRoute] string deviceId)
        {
            var selectedDevice = _userDevicesRepo.First(d => d.DeviceId == deviceId);
            if (selectedDevice == null)
            {
                return BadRequest("Kayıt bulunamadı.");
            }
            else
            {
                selectedDevice.IsLoggedin = false;
                selectedDevice.UpdateAt = DateTime.Now;

                var response = _userDevicesRepo.Update(selectedDevice);
                if (response)
                {
                    User.Logout();
                    return Ok("Cihazda oturum kapatıldı.");
                }
                else
                {
                    return BadRequest("Cihazda oturum kapatılamadı.");
                }
            }
        }

    }
}
