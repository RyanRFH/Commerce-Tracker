using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Dto.AccountDtos;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace commerce_tracker_v2.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    UserName = request.Username,
                    Email = request.Email
                };

                var createdUser = await _userManager.CreateAsync(user, request.Password);

                if (createdUser.Succeeded)
                {
                    var createdRole = await _userManager.AddToRoleAsync(user, "User");
                    if (createdRole.Succeeded)
                    {
                        return Ok(
                        new NewUserDto
                        {
                            UserName = user.UserName,
                            Email = user.Email
                        }
                        );
                    }
                    else
                    {
                        return StatusCode(500, createdRole.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception error)
            {
                return StatusCode(500, error);
            }
        }

    }
}