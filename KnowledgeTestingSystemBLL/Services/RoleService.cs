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
        /// <summary>
        /// This method assigns array of roles to user
        /// </summary>
        /// <param name="assignUserToRoles">Model with user id and array of roles</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Model is null</exception>
        /// <exception cref="Exception">List or errors(e.x: model is invalid, user not found)</exception>
        public async Task AssignUserToRoles(AssignUserToRoles assignUserToRoles)
        {
            if(assignUserToRoles == null) throw new ArgumentNullException(nameof(assignUserToRoles));

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
            if (roleName is null) throw new ArgumentNullException(nameof(roleName));

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
        /// <summary>
        /// This method returns roles for user with given email
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>List of roles</returns>
        /// <exception cref="Exception">User not found</exception>
        public async Task<IEnumerable<string>> GetRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception($"There is no such user: '{email}'");

            return (await _userManager.GetRolesAsync(user)).ToList();
        }
    }
}
