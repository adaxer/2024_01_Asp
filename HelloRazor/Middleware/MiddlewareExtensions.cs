using HelloRazor.Middleware;

namespace Microsoft.AspNetCore.Builder;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder builder) => builder.UseMiddleware<CustomRequestLogging>();
}
