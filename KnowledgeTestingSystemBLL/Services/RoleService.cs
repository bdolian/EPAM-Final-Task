using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AssignUserToRoles(AssignUserToRoles assignUserToRoles)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == assignUserToRoles.Email);
            var roles = assignUserToRoles.Roles.ToList();
            var result = await _userManager.AddToRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(';', result.Errors.Select(x => x.Description)));
            }
        }

        public async Task CreateRole(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                throw new Exception($"Role could not be created: {roleName}.");
            }
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetRoles(ApplicationUser user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }
    }
}
