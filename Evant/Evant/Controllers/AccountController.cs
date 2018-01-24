using System;
using System.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Evant.Contracts.DataTransferObjects.Account;
using Evant.Contracts.DataTransferObjects.User;
using Evant.DAL.EF.Tables;
using Evant.DAL.Interfaces.Repositories;
using Evant.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Evant.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IRepository<User> _userRepo;
        private readonly IConfiguration _configuration;


        public AccountController(IConfiguration configuration, IRepository<User> userRepo)
        {
            _userRepo = userRepo;
            _configuration = configuration;
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
                    CreatedAt = DateTime.Now,
                    Role = "User",
                    IsActive = true,
                    IsFacebook = false,
                    Photo = null,
                    FacebookId = null
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
                var token = GenerateJwtToken(selectedUser);
                return Ok(token);
            }

            return BadRequest("Böyle bir kullanıcı yok.");
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public IActionResult GetMe()
        {
            Guid userId = User.GetUserId();

            var user = _userRepo.First(u => u.Id == userId);
            if(user == null)
            {
                return BadRequest("Kullanıcı bulunamadı.");
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

        [HttpPut]
        [Authorize]
        [Route("password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDTO password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Eksik bilgi girdiniz.");
            }

            if(password.NewPassword != password.ReNewPassword)
            {
                return BadRequest("Şifreler aynı değil.");
            }

            Guid userId = User.GetUserId();
            var user = _userRepo.First(u => u.Id == userId);
            
            if(user != null)
            {
                if (user.Password == password.OldPassword)
                {
                    user.Password = password.NewPassword;
                    user.UpdateAt = DateTime.Now;

                    var response = _userRepo.Update(user);

                    if(response)
                    {
                        return Ok("Şifreniz başarıyla güncellendi.");
                    }

                    return Ok("Şifreniz güncellenemedi.");
                }

                return BadRequest("Şifrenizi hatalı girdiniz.");
            }

            return BadRequest("Böyle bir kullanıcı bulunamadı.");
        }

        [HttpGet]
        [Authorize]
        [Route("deactive")]
        public IActionResult DeActiveAccount()
        {
            Guid userId = User.GetUserId();
            var user = _userRepo.First(u => u.Id == userId);

            if(user != null)
            {
                if(user.IsActive)
                {
                    user.IsActive = false;
                    user.UpdateAt = DateTime.Now;

                    var response = _userRepo.Update(user);

                    if(response)
                    {
                        return Ok("Hesabınız başarıyla deaktif edilmiştir.");
                    }

                    return BadRequest("Hesabınız deaktif edilemedi.");
                }

                return BadRequest("Hesap zaten deaktif edilmiş.");
            }

            return BadRequest("Böyle bir kullanıcı bulunamadı.");
        }


        private object GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
