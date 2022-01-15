using KnowledgeTestingSystemBLL;
using KnowledgeTestingSystemBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KnowledgeTestingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(string role)
        {
            await _roleService.CreateRole(role);
            return Ok();
        }

        [HttpGet("getRoles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _roleService.GetRoles());
        }

        [HttpGet("getUserRoles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var roles = await _roleService.GetRoles(email);
            return Ok(roles);
        }

        [HttpPost("assignUserToRole")]
        public async Task<IActionResult> AssignUserToRole(AssignUserToRoles model)
        {
            await _roleService.AssignUserToRoles(model);

            return Ok();
        }
    }
}
