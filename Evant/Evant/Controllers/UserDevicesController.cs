﻿using System;
using System.Linq;
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
        [HttpGet]
        public async Task<IActionResult> Devices()
        {
            try
            {
                var devices = (await _userDevicesRepo.Where(u => !u.IsDeleted && u.IsLoggedin && u.UserId == User.GetUserId())).Select(d => new UserDeviceDTO()
                {
                    DeviceId = d.DeviceId,
                    Brand = d.Brand,
                    Model = d.Model,
                    OS = d.OS
                }).ToList();

                if (devices.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logHelper.Log("UserDevicesController", 500, "Devices", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveDevice([FromBody] UserDeviceDTO device)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                var selectedDevice = await _userDevicesRepo.First(d => d.DeviceId == device.DeviceId);
                if (selectedDevice != null)
                {
                    selectedDevice.IsLoggedin = true;
                    selectedDevice.UpdateAt = DateTime.UtcNow;

                    var response = await _userDevicesRepo.Update(selectedDevice);
                    if (response)
                    {
                        return Ok("Cihazınız güncellendi.");
                    }
                    else
                    {
                        return BadRequest("Cihazınız güncellenemedi.");
                    }
                }
                else
                {
                    var response = await _userDevicesRepo.Add(new UserDevice()
                    {
                        Id = new Guid(),
                        UserId = User.GetUserId(),
                        DeviceId = device.DeviceId,
                        Brand = device.Brand,
                        Model = device.Model,
                        OS = device.OS,
                        IsLoggedin = true,
                    });
                    if (response)
                    {
                        return Ok("Cihazınız eklendi.");
                    }
                    else
                    {
                        return BadRequest("Cihazınız eklenemedi.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("UserDevicesController", 500, "SaveDevice", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet("{deviceId}/logout")]
        public async Task<IActionResult> Logout([FromRoute] string deviceId)
        {
            try
            {
                var selectedDevice = await _userDevicesRepo.First(d => d.DeviceId == deviceId);
                if (selectedDevice == null)
                    return BadRequest("Kayıt bulunamadı.");

                selectedDevice.IsLoggedin = false;
                selectedDevice.UpdateAt = DateTime.UtcNow;
                var response = await _userDevicesRepo.Update(selectedDevice);
                if (response)
                {
                    return Ok("Cihazınızda oturum kapatıldı.");
                }
                else
                {
                    return BadRequest("Cihazınızda oturum kapatılamadı.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("UserDevicesController", 500, "CloseDevice", ex.Message);
                return null;
            }
        }

    }
}
