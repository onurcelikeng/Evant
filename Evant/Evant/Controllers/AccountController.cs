using System;
using Evant.Contracts.DataTransferObjects.Account;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Evant.Storage.Interfaces;
using Evant.Storage.Models;
using System.Threading.Tasks;
using Evant.Storage.Extensions;
using Evant.Contracts.DataTransferObjects.UserSettingDTO.cs;
using Evant.Auth;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<UserSetting> _userSettingRepo;
        private readonly IJwtFactory _jwtFactory;
        private readonly IConfiguration _configuration;
        private readonly IAzureBlobStorage _blobStorage;


        public AccountController(IRepository<User> userRepo,
            IRepository<UserSetting> userSettingRepo,
            IJwtFactory jwtFactory,
            IConfiguration configuration,
            IAzureBlobStorage blobStorage)
        {
            _userRepo = userRepo;
            _userSettingRepo = userSettingRepo;
            _jwtFactory = jwtFactory;
            _configuration = configuration;
            _blobStorage = blobStorage;
        }


        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Eksik bilgi girdiniz.");
            }

            var selectedUser = _userRepo.First(u => u.Email == user.Email);
            if (selectedUser != null)
            {
                return BadRequest("Eposta adresi zaten kullanılıyor.");
            }

            else
            {
                var newUser = new User
                {
                    Id = new Guid(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    Role = "User",
                    IsActive = true,
                    IsFacebook = false,
                    Photo = null,
                    FacebookId = null,
                    UserSetting = new UserSetting()
                };

                var response = _userRepo.Insert(newUser);
                if (response)
                {
                    return Ok("Kullanıcı eklendi.");
                }
                else
                {
                    return BadRequest("Kullanıcı eklenemedi.");
                }
            }
        }

        [HttpPost]
        [Route("token")]
        public IActionResult Login([FromBody] LoginDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Eksik bilgi girdiniz.");
            }

            var selectedUser = _userRepo.First(u => u.Email == user.Email && u.Password == user.Password);
            if (selectedUser != null)
            {
                var token = _jwtFactory.GenerateJwtToken(selectedUser);
                return Ok(token);
            }

            return BadRequest("Böyle bir kullanıcı yok.");
        }

        [HttpGet]
        [Route("fblogin")]
        public IActionResult FacebookLogin()
        {
            //...
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            //...
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("me")]
        public IActionResult GetMe()
        {
            Guid userId = User.GetUserId();

            var user = _userRepo.First(u => u.Id == userId);
            if (user == null)
            {
                return BadRequest("Kayıt bulunamadı.");
            }
            else
            {
                var model = new UserDetailDTO()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhotoUrl = user.Photo
                };

                return Ok(model);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("photo")]
        public async Task<IActionResult> UploadPhoto(FileInputModel inputModel)
        {
            try
            {
                if (inputModel == null)
                    return BadRequest("Argument null");

                if (inputModel.File == null || inputModel.File.Length == 0)
                    return BadRequest("file not selected");

                Guid userId = User.GetUserId();
                var blobName = userId + "_" + inputModel.File.GetFilename();
                var fileStream = await inputModel.File.GetFileStream();

                var isUploaded = await _blobStorage.UploadAsync(blobName, fileStream);
                if (isUploaded)
                {
                    var selectedUser = _userRepo.First(u => u.Id == userId);
                    selectedUser.UpdateAt = DateTime.Now;
                    selectedUser.Photo = "https://evantstorage.blob.core.windows.net/users/" + blobName;

                    var resposne = _userRepo.Update(selectedUser);
                    if (resposne)
                        return Ok("photo uploaded.");
                    else
                        return BadRequest("photo not uploaded.");
                }
                else
                {
                    return Ok("photo uploaded.");
                }
            }
            catch (Exception)
            {
                return BadRequest("photo not uploaded.");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("settings")]
        public IActionResult ChangeUserSetting([FromBody] UserSettingDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Eksik bilgi girdiniz.");

            Guid userId = User.GetUserId();
            var selectedUserSetting = _userSettingRepo.First(us => us.UserId == userId);
            if (selectedUserSetting == null)
            {
                return NotFound("Kayıt bulunamadı.");
            }
            else
            {
                selectedUserSetting.UpdateAt = DateTime.Now;
                selectedUserSetting.Theme = model.Theme;
                selectedUserSetting.Language = model.Language;
                selectedUserSetting.IsCommentNotif = model.IsCommentNotif;
                selectedUserSetting.IsEventNewComerNotif = model.IsEventNewComerNotif;
                selectedUserSetting.IsEventUpdateNotif = model.IsEventUpdateNotif;
                selectedUserSetting.IsFriendshipNotif = model.IsFriendshipNotif;

                var response = _userSettingRepo.Update(selectedUserSetting);
                if (response)
                {
                    return Ok("Kullanıcı ayarları güncellendi.");
                }
                else
                {
                    return BadRequest("Kullanıcı ayarları güncellenemedi.");
                }
            }
        }

        [Authorize]
        [HttpPut]
        [Route("password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDTO password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Eksik bilgi girdiniz.");
            }

            if (password.NewPassword != password.ReNewPassword)
            {
                return BadRequest("Şifreler aynı değil.");
            }

            Guid userId = User.GetUserId();
            var user = _userRepo.First(u => u.Id == userId);

            if (user != null)
            {
                if (user.Password == password.OldPassword)
                {
                    user.Password = password.NewPassword;
                    user.UpdateAt = DateTime.Now;

                    var response = _userRepo.Update(user);

                    if (response)
                    {
                        return Ok("Şifreniz başarıyla güncellendi.");
                    }

                    return Ok("Şifreniz güncellenemedi.");
                }

                return BadRequest("Şifrenizi hatalı girdiniz.");
            }

            return BadRequest("Böyle bir kullanıcı bulunamadı.");
        }

        [Authorize]
        [HttpGet]
        [Route("deactive")]
        public IActionResult DeActiveAccount()
        {
            Guid userId = User.GetUserId();
            var user = _userRepo.First(u => u.Id == userId);

            if (user != null)
            {
                if (user.IsActive)
                {
                    user.IsActive = false;
                    user.UpdateAt = DateTime.Now;

                    var response = _userRepo.Update(user);

                    if (response)
                    {
                        return Ok("Hesabınız başarıyla deaktif edilmiştir.");
                    }

                    return BadRequest("Hesabınız deaktif edilemedi.");
                }

                return BadRequest("Hesap zaten deaktif edilmiş.");
            }

            return BadRequest("Kayıt bulunamadı.");
        }

    }
}
