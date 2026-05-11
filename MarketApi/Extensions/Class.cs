using MarketApi.Middleware;

namespace MarketApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtSessionValidation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtSessionValidationMiddleware>();
        }
    }
}
