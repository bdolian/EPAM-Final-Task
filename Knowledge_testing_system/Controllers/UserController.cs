﻿using KnowledgeTestingSystem.Models;
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
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("me")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetMeAsync()
        {
            string email = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userService.GetAsync(x => x.Email == email);

            var userInfo = await _userService.GetWithProfileAsync(user.First().Id);

            return Ok(userInfo);
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser(UserModel user)
        {
            var isDeleted = await _userService.DeleteAsync(new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            });

            if (!isDeleted)
                throw new ArgumentException("You passed invalid user, it is not deleted");

            return Ok();
        }

        [HttpPut("editUser")]
        public async Task<IActionResult> EditUser(UserModel newUser)
        {
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            await _userService.EditAsync(new UserDTO
            {
                Id = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email
            });

            return NoContent();
        }
    }
}
