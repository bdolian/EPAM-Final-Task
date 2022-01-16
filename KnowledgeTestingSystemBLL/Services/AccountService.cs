using AutoMapper;
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
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager, IUserService userService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userService = userService;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Register, ApplicationUser>().ReverseMap();
                cfg.CreateMap<Register, UserDTO>().ReverseMap();
            });

            _mapper = new Mapper(config);
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplicationUser> Logon(Logon logonUser)
        {
            if(logonUser == null) throw new ArgumentNullException(nameof(logonUser));

            var user = _userManager.Users.SingleOrDefault(u => u.Email == logonUser.Email);
            if (user is null) throw new Exception($"User not found: '{logonUser.Email}'.");

            var userInApp = _unitOfWork.UserRepository.GetByEmailAsync(logonUser.Email);
            if (userInApp is null) throw new Exception($"User is deleted: '{logonUser.Email}'");

            return await _userManager.CheckPasswordAsync(user, logonUser.Password) ? user : null;
        }

        public async Task Register(Register user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            var userToRegister = _mapper.Map<Register, ApplicationUser>(user);
            userToRegister.UserName = user.Email;
            var result = await _userManager.CreateAsync(userToRegister, user.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(';', result.Errors.Select(x => x.Description)));
            }

            await _userManager.AddToRoleAsync(userToRegister, "user");

            var newUser = _mapper.Map<Register, UserDTO>(user);

            await _userService.CreateAsync(newUser);
        }

        public async Task<IdentityResult> DeleteUser(ApplicationUser user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            return await _userManager.DeleteAsync(user);
        }
    }
}
