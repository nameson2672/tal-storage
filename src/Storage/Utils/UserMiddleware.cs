using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TalStorage.Utils
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                context.Items["UpdatedBy"] = context.User.Identity.Name;
            }

            await _next(context);
        }
    }
}
