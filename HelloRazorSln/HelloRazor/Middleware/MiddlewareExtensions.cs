namespace HelloRazor.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder builder) => builder.UseMiddleware<CustomRequestLogging>();
}
