using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using commerce_tracker_v2.Dto.AccountDtos;
using commerce_tracker_v2.Interfaces;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace commerce_tracker_v2.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
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

                var newClaims = new List<Claim>
                {
                    new("Email", request.Email)
                };

                await _userManager.AddClaimsAsync(user, newClaims);

                if (createdUser.Succeeded)
                {
                    var createdRole = await _userManager.AddToRoleAsync(user, "User");

                    if (createdRole.Succeeded)
                    {
                        newClaims.Add(new Claim(ClaimTypes.Role, "Admin"));

                        return Ok(
                            new NewUserDto
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                // Token = _tokenService.CreateToken(appUser)
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


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null)
            {
                return Unauthorized("Information incorrect");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Information incorrect");
            }

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );


        }




    }
}