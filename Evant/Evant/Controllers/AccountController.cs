﻿using System;
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
using Evant.Contracts.DataTransferObjects.Timeline;
using System.Collections.Generic;
using System.Linq;
using Evant.Contracts.DataTransferObjects.Business;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IUserRepository _userRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IEventOperationRepository _eventOperationRepo;
        private readonly IFriendOperationRepository _friendOperationRepo;
        private readonly IAzureBlobStorage _blobStorage;
        private readonly ILogHelper _logHelper;
        private readonly IJwtFactory _jwtFactory;


        public AccountController(IUserRepository userRepo,
            IEventRepository eventRepo,
            ICommentRepository commentRepo,
            IFriendOperationRepository friendOperationRepo,
            IEventOperationRepository eventOperationRepo,
            ILogHelper logHelper,
            IJwtFactory jwtFactory,
            IAzureBlobStorage blobStorage)
        {
            _userRepo = userRepo;
            _eventRepo = eventRepo;
            _commentRepo = commentRepo;
            _friendOperationRepo = friendOperationRepo;
            _eventOperationRepo = eventOperationRepo;
            _logHelper = logHelper;
            _jwtFactory = jwtFactory;
            _blobStorage = blobStorage;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                var selectedUser = await _userRepo.First(u => u.Email == user.Email);
                if (selectedUser != null)
                {
                    return BadRequest("Eposta adresi zaten kullanılıyor.");
                }
                else
                {
                    var response = await _userRepo.Add(new User
                    {
                        Id = new Guid(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password
                    });
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
                _logHelper.Log("AccountController", 500, "Register", ex.Message);
                return null;
            }
        }

        [HttpPost("token")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                var selectedUser = await _userRepo.Login(user.Email, user.Password);
                if (selectedUser != null)
                {
                    if (!selectedUser.IsActive)
                    {
                        selectedUser.IsActive = true;
                        selectedUser.UpdateAt = DateTime.UtcNow;
                        await _userRepo.Update(selectedUser);
                    }

                    return Ok(new TokenDTO()
                    {
                        Token = _jwtFactory.GenerateJwtToken(selectedUser)
                    });
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
                        return NotFound("Kayıtlı e-posta adresi bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.Log("AccountController", 500, "Login", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            try
            {
                var user = await _userRepo.GetUser(User.GetUserId());
                if (user == null)
                    return BadRequest("Kayıt bulunamadı.");

                var model = new UserDetailDTO()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhotoUrl = user.Photo,
                    IsBusiness = user.IsBusinessAccount,
                    FollowersCount = user.Followers.Count,
                    FollowingsCount = user.Followings.Count,
                    Settings = new UserSettingInfoDTO()
                    {
                        Language = user.Setting.Language,
                        Theme = user.Setting.Theme
                    },
                    Business = new BusinessInfoDTO()
                    {
                        Type = user.Business.BusinessType,
                        ExpireAt = user.Business.ExpireDate,
                        IsAgeAnalysis = user.Business.IsAgeAnalysis,
                        IsAttendedUserAnalysis = user.Business.IsAttendedUserAnalysis,
                        IsChatBotSupport = user.Business.IsChatBotSupport,
                        IsCommentAnalysis = user.Business.IsCommentAnalysis,
                        IsSendNotificationUsers = user.Business.IsSendNotificationUsers
                    }
                };

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logHelper.Log("AccountController", 500, "GetMe", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet("timeline")]
        public async Task<IActionResult> Timeline()
        {
            try
            {
                Guid userId = User.GetUserId();
                List<TimelineDTO> timeline = new List<TimelineDTO>();

                //User Events
                var eventsTimeline = (await _eventRepo.UserEvents(userId)).Select(e => new TimelineDTO()
                {
                    Header = e.Title,
                    Body = TimelineHelper.GenerateMyCreateEventBody(e),
                    Image = e.Photo,
                    CreateAt = e.CreatedAt,
                    CustomId = e.Id,
                    Type = "create-event",
                    LineColor = "#a6714e"
                }).ToList();
                if (!eventsTimeline.IsNullOrEmpty())
                {
                    timeline.AddRange(eventsTimeline);
                }

                //Event Operations
                var eventOperationsTimeline = (await _eventOperationRepo.UserEventOperations(userId)).Select(eo => new TimelineDTO()
                {
                    Header = eo.Event.Title,
                    Body = TimelineHelper.GenerateMyJoinEventBody(eo.Event),
                    Image = eo.Event.Photo,
                    CreateAt = eo.CreatedAt,
                    CustomId = eo.EventId,
                    Type = "join-event",
                    LineColor = "#f78f8f"
                }).ToList();
                if (!eventOperationsTimeline.IsNullOrEmpty())
                {
                    timeline.AddRange(eventOperationsTimeline);
                }

                //Following Friend Operations
                var followFriendOperationsTimeline = (await _friendOperationRepo.Followings(userId)).Select(fo => new TimelineDTO()
                {
                    Header = fo.FollowingUser.FirstName + " " + fo.FollowingUser.LastName,
                    Body = TimelineHelper.GenerateMyFollowingBody(),
                    Image = null,
                    CreateAt = fo.CreatedAt,
                    CustomId = fo.FollowingUserId,
                    Type = "following",
                    LineColor = "#ffeea3"
                }).ToList();
                if (!followFriendOperationsTimeline.IsNullOrEmpty())
                {
                    timeline.AddRange(followFriendOperationsTimeline);
                }

                //Follow Friend Operations
                var followingFriendOperationsTimeline = (await _friendOperationRepo.Followers(userId)).Select(fo => new TimelineDTO()
                {
                    Header = fo.FollowerUser.FirstName + " " + fo.FollowerUser.LastName,
                    Body = TimelineHelper.GenerateMyFollowerBody(),
                    Image = null,
                    CreateAt = fo.CreatedAt,
                    CustomId = fo.FollowerUserId,
                    Type = "follower",
                    LineColor = "#ffeea3"
                }).ToList();
                if (!followingFriendOperationsTimeline.IsNullOrEmpty())
                {
                    timeline.AddRange(followingFriendOperationsTimeline);
                }

                //Comments
                var commentsTimeline = (await _commentRepo.UserComments(userId)).Select(c => new TimelineDTO()
                {
                    Header = c.Event.Title,
                    Body = TimelineHelper.GenerateMyCommentBody(userId, c),
                    Image = null,
                    CreateAt = c.CreatedAt,
                    CustomId = c.EventId,
                    Type = "comment-event",
                    LineColor = "#dcd5f2"
                }).ToList();
                if (!commentsTimeline.IsNullOrEmpty())
                {
                    timeline.AddRange(commentsTimeline);
                }

                if (timeline.IsNullOrEmpty())
                    return NotFound("Kayıt bulunamadı.");

                return Ok(timeline.OrderByDescending(t => t.CreateAt));
            }
            catch (Exception ex)
            {
                _logHelper.Log("AccountController", 500, "Timeline", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost("photo")]
        public async Task<IActionResult> UploadPhoto([FromForm] FileInputModel inputModel)
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

                var isUploaded = await _blobStorage.UploadAsync("user", blobName, fileStream);
                if (isUploaded)
                {
                    var selectedUser = await _userRepo.GetUser(userId);
                    selectedUser.UpdateAt = DateTime.UtcNow;
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
                _logHelper.Log("AccountController", 500, "UploadPhoto", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPost("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDTO user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                var selectedUser = await _userRepo.First(u => u.Id == User.GetUserId());
                if (selectedUser == null)
                    return NotFound("Kayıt bulunamadı.");

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
                selectedUser.Birthdate = user.Birthdate;
                selectedUser.UpdateAt = DateTime.UtcNow;

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
            catch (Exception ex)
            {
                _logHelper.Log("AccountController", 500, "UpdateProfile", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO password)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Eksik bilgi girdiniz.");

                if (password.NewPassword != password.ReNewPassword)
                    return BadRequest("Şifreler aynı değil.");

                var user = await _userRepo.First(u => u.Id == User.GetUserId());
                if (user != null)
                {
                    if (user.Password == password.OldPassword)
                    {
                        user.Password = password.NewPassword;
                        user.UpdateAt = DateTime.UtcNow;

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
                _logHelper.Log("AccountController", 500, "ChangePassword", ex.Message);
                return null;
            }
        }

        [Authorize]
        [HttpGet("deactive")]
        public async Task<IActionResult> DeActiveAccount()
        {
            try
            {
                var user = await _userRepo.GetUser(User.GetUserId());
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        user.IsActive = false;
                        user.UpdateAt = DateTime.UtcNow;

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
                _logHelper.Log("AccountController", 500, "DeActiveAccount", ex.Message);
                return null;
            }
        }

    }
}
