using Microsoft.AspNetCore.Mvc;

namespace TalStorage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IActionResult SuccessResponse(object data, string message = null)
        {
            return Ok(new { success = true, message, data });
        }

        protected IActionResult ErrorResponse(string message, int statusCode)
        {
            return StatusCode(statusCode, new { success = false, message });
        }
    }
}
