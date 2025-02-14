using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;
using TalStorage.DTOs;
using TalStorage.Models;
using TalStorage.Service;
using TalStorage.Utils;

namespace TalStorage.Controllers
{
    public class AccountController : BaseController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email.ToLower(),
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
        // Request password reset
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] TalStorage.DTOs.ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            // Get domain from appsettings.json
            var domain = _configuration["AppSettings:Domain"];
            var resetLink = $"{domain}/reset-password?email={user.Email}&token={encodedToken}";

            // Send email
            string emailBody = $"<h1>Reset Your Password</h1><p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";
            await _emailService.SendEmailAsync(user.Email, "Reset Your Password", emailBody);

            return Ok(new { message = "Password reset link sent to your email." });
        }

        // Reset password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] TalStorage.DTOs.ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Email, token, and new password are required.");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Password reset successful." });
        }
    }
}
