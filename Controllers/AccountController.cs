using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using commerce_tracker_v2.Data;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager, DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _context = context;
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

                //Check role exists
                if (request.Role == "User" || request.Role == "Admin")
                {

                }
                else
                {
                    return BadRequest("Invalid user role");
                }


                var user = new User
                {
                    UserName = request.Username,
                    Email = request.Email
                };

                var createdUser = await _userManager.CreateAsync(user, request.Password);

                //Claims test
                //Claims are used to add key/values to the user (some data keys are not included in the microsoft IdentityUser model,
                //such as firstname and lastname)
                var newClaims = new List<Claim>
                {
                    new("Email", request.Email) //For testing
                    // new("FirstName", request.FirstName)
                    // new("LastName", request.LastName)
                };

                await _userManager.AddClaimsAsync(user, newClaims);

                if (createdUser.Succeeded)
                {
                    if (request.Role == "User")
                    {
                        var role = await _roleManager.FindByNameAsync("User");
                        if (role == null)
                        {
                            role = new IdentityRole("User");
                            await _roleManager.CreateAsync(role);
                        }
                        var createdRole = await _userManager.AddToRoleAsync(user, "User");

                        if (createdRole.Succeeded)
                        {
                            newClaims.Add(new Claim(ClaimTypes.Role, "User"));

                            return Ok(
                                new NewUserDto
                                {
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    Role = "User",
                                    Token = _tokenService.CreateToken(user, request.Role)
                                }
                            );
                        }
                        else
                        {
                            return StatusCode(500, createdRole.Errors);
                        }

                    }
                    else if (request.Role == "Admin")
                    {
                        var role = await _roleManager.FindByNameAsync("Admin");
                        if (role == null)
                        {
                            role = new IdentityRole("Admin");
                            await _roleManager.CreateAsync(role);
                        }
                        var createdRole = await _userManager.AddToRoleAsync(user, "Admin");

                        if (createdRole.Succeeded)
                        {
                            newClaims.Add(new Claim(ClaimTypes.Role, "Admin"));

                            return Ok(
                                new NewUserDto
                                {
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    Role = "Admin",
                                    Token = _tokenService.CreateToken(user, request.Role)
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
                        return BadRequest("Invalid user role");
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
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null)
            {
                return Unauthorized(new { error = "Information Incorrect" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { error = "Information Incorrect" });
            }

            List<string> userRolesIds = _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).ToList();

            List<string> userRoles = new List<string>();
            if (userRolesIds != null)
            {

                foreach (var roleId in userRolesIds)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);
                    userRoles.Add(role.Name);
                };

            }
            else
            {
                return BadRequest("User does not have a role assigned");
            }
            if (userRoles.Count == 0)
            {
                return BadRequest("User does not have a role assigned");
            }

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = userRoles[0],
                    Token = _tokenService.CreateToken(user, userRoles[0]) //Accounts should only have one role at present so only sending one
                }
            );


        }


        [HttpGet("user")]
        public async Task<IActionResult> GetUser([FromHeader] string userJWT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(userJWT))
            {
                return BadRequest("This is not a valid JWT");
            }
            var jwt = tokenHandler.ReadJwtToken(userJWT);

            var claims = jwt.Claims;
            var user = new
            {
                userName = claims.First(claim => claim.Type == "given_name").Value,
                email = claims.First(claim => claim.Type == "email").Value,
                role = claims.First(claim => claim.Type == "role").Value,
                id = claims.First(claim => claim.Type == "user_id").Value,
            };

            return Ok(user);
        }


    }
}