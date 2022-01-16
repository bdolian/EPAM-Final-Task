using AutoMapper;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KnowledgeTestingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "user, admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IRoleService roleService, IAccountService accountService)
        {
            _userService = userService;
            _roleService = roleService;
            _accountService = accountService;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, ApplicationUser>().ReverseMap();
            });
            _mapper = new Mapper(config);
        }

        [HttpGet("getUsers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMeAsync()
        {
            string email = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userService.GetAsync(x => x.Email == email);

            var userInfo = await _userService.GetWithProfileAsync(user.First().Id);

            return Ok(userInfo);
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            string email = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userService.GetAsync(x => x.Email == email);
            bool isAdmin = false;
            foreach(var item in await _roleService.GetRoles(email))
            {
                if(item == "admin")
                    isAdmin = true;
            }
            if (user.ElementAt(0).Id == id || isAdmin)
            {
                var isDeleted = await _userService.DeleteAsync(id);

                if (!isDeleted)
                    return BadRequest("You passed invalid user, it is not deleted");
                return Ok();
            }
            else
                return Forbid();
        }

        [HttpPut("editUser")]
        public async Task<IActionResult> EditUser(UserCompleteInformation newUser)
        {
            if (newUser == null)
                return BadRequest("You passed no user");

            string email = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userService.GetAsync(x => x.Email == email);

            if (user.ElementAt(0).Id == newUser.User.Id)
            {
                await _userService.EditCompleteAsync(newUser);

                return NoContent();
            }
            else
                return Forbid("You can't edit other user info");
        }

    }
}
