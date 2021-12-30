using KnowledgeTestingSystem.Models.Account;
using KnowledgeTestingSystemBLL;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KnowledgeTestingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost("assignUserToRole")]
        public async Task<IActionResult> AssignUserToRole(AssignUserToRoleModel model)
        {
            await _roleService.AssignUserToRoles(new AssignUserToRoles
            {
                Email = model.Email,
                Roles = model.Roles
            });

            return Ok();
        }
    }
}
