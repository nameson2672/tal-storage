using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalStorage.DTOs;

namespace TalStorage.Controllers
{
    public class UsersController : BaseController
    {
        private readonly FileUploadDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(FileUploadDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var data = _httpContextAccessor.HttpContext?.User;
            var user = await _context.Users.FindAsync(_httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value);
            if (user == null) return ErrorResponse("User not found", 404);

            return SuccessResponse(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate,
                UpdatedBy = user.UpdatedBy
            });
        }

        [HttpGet("{emailPrefix}")]
        [Authorize]
        public async Task<IActionResult> GetUsersByEmail(string emailPrefix)
        {
            var users = await _context.Users
                               .Where(u => u.Email.StartsWith(emailPrefix.ToLower()))
                               .Select(x => new { Name = x.FullName, Email = x.Email, Id = x.Id })
                               .ToListAsync();

            return SuccessResponse(users);
        }
    }
}
