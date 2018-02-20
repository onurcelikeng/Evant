using System;
using Evant.Contracts.DataTransferObjects.Account;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Evant.Storage.Interfaces;
using Evant.Storage.Models;
using System.Threading.Tasks;
using Evant.Storage.Extensions;
using Evant.Auth;
using Evant.DAL.Repositories.Interfaces;
using Evant.Interfaces;
using Evant.Contracts.DataTransferObjects.UserSettingDTO;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogHelper _logHelper;
        private readonly IJwtFactory _jwtFactory;
        private readonly IAzureBlobStorage _blobStorage;


        public AccountController(IUserRepository userRepo,
            ILogHelper logHelper,
            IJwtFactory jwtFactory,
            IAzureBlobStorage blobStorage)
        {
            _userRepo = userRepo;
            _logHelper = logHelper;
            _jwtFactory = jwtFactory;
            _blobStorage = blobStorage;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Eksik bilgi girdiniz.");
                }

                var selectedUser = await _userRepo.First(u => u.Email == user.Email);
                if (selectedUser != null)
                {
                    return BadRequest("Eposta adresi zaten kullanılıyor.");
                }
                else
                {
                    var entity = new User
                    {
                        Id = new Guid(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password,
                        Role = "User",
                        IsActive = true,
                        IsFacebook = false,
                        Photo = "https://evantstorage.blob.core.windows.net/users/default.jpeg",
                        FacebookId = null,
                        Setting = new UserSetting()
                    };

                    var response = await _userRepo.Add(entity);
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
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "Register", ex.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Eksik bilgi girdiniz.");
                }

                var selectedUser = await _userRepo.Login(user.Email, user.Password);
                if (selectedUser != null)
                {
                    var token = _jwtFactory.GenerateJwtToken(selectedUser);
                    return Ok(token);
                }
                else
                {
                    var response = await _userRepo.First(u => u.Email == user.Email);
                    if (response != null)
                    {
                        return BadRequest("Şifreniz yanlış.");
                    }
                    else
                    {
                        return NotFound("Kayıt bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "Login", ex.Message);
                return null;
            }
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
        [Route("me")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                Guid userId = User.GetUserId();

                var user = await _userRepo.GetUser(userId);
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
                        PhotoUrl = user.Photo,
                        FollowersCount = user.Followers.Count,
                        FollowingsCount = user.Followings.Count,
                        Settings = new UserSettingInfoDTO()
                        {
                            Language = user.Setting.Language,
                            Theme = user.Setting.Theme
                        }
                    };

                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "GetMe", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("photo")]
        public async Task<IActionResult> UploadPhoto([FromBody] FileInputModel inputModel)
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
                    var selectedUser = await _userRepo.GetUser(userId);
                    selectedUser.UpdateAt = DateTime.Now;
                    selectedUser.Photo = "https://evantstorage.blob.core.windows.net/users/" + blobName;

                    var response = await _userRepo.Update(selectedUser);
                    if (response)
                        return Ok("photo uploaded.");
                    else
                        return BadRequest("photo not uploaded.");
                }
                else
                {
                    return Ok("photo uploaded.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "UploadPhoto", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDTO user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                Guid userId = User.GetUserId();

                var selectedUser = await _userRepo.First(u => u.Id == userId);
                if (selectedUser == null)
                {
                    return NotFound("Kayıt bulunamadı.");
                }
                else
                {
                    if (selectedUser.Email != user.Email)
                    {
                        bool emailExist = await _userRepo.EmailCheck(user.Email);
                        if (emailExist)
                        {
                            return BadRequest("Eposta adresi başka bir kullanıcı tarafından kullanılıyor.");
                        }
                        else
                        {
                            selectedUser.Email = user.Email;
                        }
                    }

                    selectedUser.FirstName = user.FirstName;
                    selectedUser.LastName = user.LastName;
                    selectedUser.UpdateAt = DateTime.Now;

                    var response = await _userRepo.Update(selectedUser);
                    if (response)
                    {
                        return Ok("Kullanıcı bilgileri güncellendi.");
                    }
                    else
                    {
                        return BadRequest("Kullanıcı bilgileri güncellenemedi.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "UpdateProfile", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO password)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                if (password.NewPassword != password.ReNewPassword)
                    return BadRequest("Şifreler aynı değil.");

                Guid userId = User.GetUserId();
                var user = await _userRepo.First(u => u.Id == userId);
                if (user != null)
                {
                    if (user.Password == password.OldPassword)
                    {
                        user.Password = password.NewPassword;
                        user.UpdateAt = DateTime.Now;

                        var response = await _userRepo.Update(user);
                        if (response)
                        {
                            return Ok("Şifreniz başarıyla güncellendi.");
                        }

                        return Ok("Şifreniz güncellenemedi.");
                    }

                    return BadRequest("Şifrenizi hatalı girdiniz.");
                }

                return BadRequest("Kayıt bulunamadı.");
            }
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "ChangePassword", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("deactive")]
        public async Task<IActionResult> DeActiveAccount()
        {
            try
            {
                Guid userId = User.GetUserId();

                var user = await _userRepo.GetUser(userId);
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        user.IsActive = false;
                        user.UpdateAt = DateTime.Now;

                        var response = await _userRepo.Update(user);
                        if (response)
                        {
                            return Ok("Hesabınız başarıyla deaktif edilmiştir.");
                        }
                        else
                        {
                            return BadRequest("Hesabınız deaktif edilemedi.");
                        }
                    }
                    else
                    {
                        return BadRequest("Hesap zaten deaktif edilmiş.");
                    }
                }
                else
                {
                    return BadRequest("Kayıt bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("Users", 500, "DeActiveAccount", ex.Message);
                return null;
            }
        }

    }
}
