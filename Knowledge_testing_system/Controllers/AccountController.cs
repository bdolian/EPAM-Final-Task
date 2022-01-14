using KnowledgeTestingSystem.Filters;
using KnowledgeTestingSystem.Helpers;
using KnowledgeTestingSystem.Models.Account;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace KnowledgeTestingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ModelStateActionFilter]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly JwtSettings _jwtSettings;
        
        public AccountController(IAccountService accountService, 
                                IOptionsSnapshot<JwtSettings> jwtSettings, 
                                IRoleService roleService)
        {
            _accountService = accountService;
            _jwtSettings = jwtSettings.Value;
            _roleService = roleService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            await _accountService.Register(new Register
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password,
            });

            return Created(string.Empty, string.Empty);
        }

        [HttpPost("logon")]
        public async Task<IActionResult> Logon(LogonModel model)
        {
            var user = await _accountService.Logon(new Logon
            {
                Email = model.Email,
                Password = model.Password
            });

            if (user is null) return BadRequest();

            var roles = await _roleService.GetRoles(model.Email);

            var token = JwtHelper.GenerateJwt(user, roles, _jwtSettings);

            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", token,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(30),
                SameSite = SameSiteMode.None,
                Secure = true
            });

            return Ok();
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", "",
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMilliseconds(1),
                    SameSite = SameSiteMode.None,
                    Secure = true
                });

            return Ok();
        }

    }
}
