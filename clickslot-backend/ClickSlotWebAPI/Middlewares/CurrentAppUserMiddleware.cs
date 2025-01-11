using ClickSlotCore.Contracts.Interfaces.Entity;

namespace ClickSlotWebAPI.Middlewares
{
    public class CurrentAppUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentAppUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAppUserService appUserService)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "AppUserId")?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var user = await appUserService.GetByIdAsync(userId);

                    context.Items["CurrentUser"] = user;
                }
            }

            await _next(context);
        }
    }
}