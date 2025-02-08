using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TalStorage.DTOs;
using TalStorage.Models;
using TalStorage.Utils;

namespace TalStorage.Controllers
{
    public class AccountController : BaseController
    {

            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IConfiguration _configuration;

            public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
            {
                _userManager = userManager;
                _configuration = configuration;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
            {
                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FullName = registerDto.FullName
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                    return ErrorResponse(string.Join(", ", result.Errors.Select(e => e.Description)), 400);

                return SuccessResponse(null, "Registration successful");
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                    return ErrorResponse("Invalid credentials", 401);

                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!result)
                    return ErrorResponse("Invalid credentials", 401);

                var token = JwtTokenHelper.GenerateToken(user, _configuration);
                return SuccessResponse(new { Token = token }, "Login successful");
            }
        }
    }
