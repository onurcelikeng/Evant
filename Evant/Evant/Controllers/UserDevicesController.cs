using System;
using System.Threading.Tasks;
using Evant.Contracts.DataTransferObjects.UserDevice;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Evant.Interfaces;
using Evant.NotificationCenter.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/devices")]
    public class UserDevicesController : BaseController
    {
        private readonly IRepository<UserDevice> _userDevicesRepo;
        private readonly ILogHelper _logHelper;
        private readonly IOneSignal _oneSignal;


        public UserDevicesController(IRepository<UserDevice> userDevicesRepo,
            ILogHelper logHelper,
            IOneSignal oneSignal)
        {
            _userDevicesRepo = userDevicesRepo;
            _logHelper = logHelper;
            _oneSignal = oneSignal;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveDevice([FromBody] UserDeviceDTO device)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Eksik bilgi girdiniz.");
                }

                var selectedDevice = await _userDevicesRepo.First(d => d.DeviceId == device.DeviceId);
                if (selectedDevice != null)
                {
                    selectedDevice.IsLoggedin = true;
                    selectedDevice.UpdateAt = DateTime.Now;

                    var response = await _userDevicesRepo.Update(selectedDevice);
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
                    var entity = new UserDevice()
                    {
                        Id = new Guid(),
                        UserId = userId,
                        DeviceId = device.DeviceId,
                        Brand = device.Brand,
                        Model = device.Model,
                        OS = device.OS,
                        IsLoggedin = true,
                    };

                    var response = await _userDevicesRepo.Add(entity);
                    if (response)
                    {
                        return Ok("Cihazınız eklendi.");
                    }
                    else
                    {
                        return BadRequest("Cihaz eklenemedi.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("UserDevices", 500, "SaveDevice", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("logout/{deviceId}")]
        public async Task<IActionResult> CloseDevice([FromRoute] string deviceId)
        {
            try
            {
                var selectedDevice = await _userDevicesRepo.First(d => d.DeviceId == deviceId);
                if (selectedDevice == null)
                {
                    return BadRequest("Kayıt bulunamadı.");
                }
                else
                {
                    selectedDevice.IsLoggedin = false;
                    selectedDevice.UpdateAt = DateTime.Now;

                    var response = await _userDevicesRepo.Update(selectedDevice);
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
            catch (Exception ex)
            {
                _logHelper.Log("UserDevices", 500, "CloseDevice", ex.Message);
                return null;
            }
        }

    }
}
