using MarketData.Abstract;
namespace MarketApi.Middleware
{
    public class JwtSessionValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtSessionValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserSessionRepository userSessionRepository)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // auth login/register ve swagger için kontrol yapma
            if (path.Contains("/api/auth/login") ||
                path.Contains("/api/auth/register") ||
                path.Contains("/swagger"))
            {
                await _next(context);
                return;
            }

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userIdStr = context.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                var sessionToken = context.User.Claims.FirstOrDefault(c => c.Type == "sessionToken")?.Value;

                if (!long.TryParse(userIdStr, out var userId) || string.IsNullOrWhiteSpace(sessionToken))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        code = "401",
                        message = "Geçersiz oturum.",
                        errors = new[] { "Session bilgisi bulunamadı." }
                    });
                    return;
                }

                var session = userSessionRepository.Get(x =>
                    x.UserId == userId &&
                    x.SessionToken == sessionToken &&
                    x.Status == true &&
                    x.IsActive == true);

                if (session == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        code = "401",
                        message = "Oturum bulunamadı veya kapatılmış.",
                        errors = new[] { "Lütfen tekrar giriş yapın." }
                    });
                    return;
                }

                if (session.ExpireAt <= DateTime.UtcNow)
                {
                    session.IsActive = false;
                    session.ModifiedDate = DateTime.UtcNow;
                    userSessionRepository.Update(session);

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        code = "401",
                        message = "Oturum süresi dolmuş.",
                        errors = new[] { "Lütfen tekrar giriş yapın." }
                    });
                    return;
                }
            }

            await _next(context);
        }
    }
}
