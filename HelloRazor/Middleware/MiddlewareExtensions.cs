using HelloRazor.Middleware;

namespace Microsoft.AspNetCore.Builder;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder) => builder.UseMiddleware<RequestLogging>();
}
