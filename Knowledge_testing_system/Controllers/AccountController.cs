using KnowledgeTestingSystem.Filters;
using KnowledgeTestingSystem.Helpers;
using KnowledgeTestingSystem.Models.Account;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace KnowledgeTestingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ModelStateActionFilter]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly JwtSettings _jwtSettings;
        
        public AccountController(IUserService userService, IOptionsSnapshot<JwtSettings> jwtSettings, IRoleService roleService)
        {
            _userService = userService;
            _jwtSettings = jwtSettings.Value;
            _roleService = roleService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            await _userService.Register(new Register
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
            var user = await _userService.Logon(new Logon
            {
                Email = model.Email,
                Password = model.Password
            });

            if (user is null) return BadRequest();

            var roles = await _roleService.GetRoles(user);

            return Ok(JwtHelper.GenerateJwt(user, roles, _jwtSettings));
        }

    }
}
