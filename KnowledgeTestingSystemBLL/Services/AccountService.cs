using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Interfaces;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public AccountService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IUserService userService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<ApplicationUser> Logon(Logon logonUser)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == logonUser.Email);
            if (user is null) throw new Exception($"User not found: '{logonUser.Email}'.");

            return await _userManager.CheckPasswordAsync(user, logonUser.Password) ? user : null;
        }

        public async Task Register(Register user)
        {
            var userToRegister = new ApplicationUser
            {
                Email = user.Email,
                UserName = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            var result = await _userManager.CreateAsync(userToRegister, user.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(';', result.Errors.Select(x => x.Description)));
            }

            await _userManager.AddToRoleAsync(userToRegister, "user");

            var newUser = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            await _userService.CreateAsync(newUser);
        }
    }
}
