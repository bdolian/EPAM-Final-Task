using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Interfaces;
using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplicationUser> Logon(Logon logonUser)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == logonUser.Email);
            if (user is null) throw new Exception($"User not found: '{logonUser.Email}'.");

            return await _userManager.CheckPasswordAsync(user, logonUser.Password) ? user : null;
        }

        public async Task Register(Register user)
        {
            var result = await _userManager.CreateAsync(new ApplicationUser
            {
                Email = user.Email,
                UserName = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }, user.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(';', result.Errors.Select(x => x.Description)));
            }

            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            await _unitOfWork.UserRepository.AddAsync(newUser);

            await _unitOfWork.UserProfileRepository.AddAsync(new UserProfile
            {
                UserId = newUser.Id,
                DateOfBirth = DateTime.MinValue
            });
        }
    }
}
