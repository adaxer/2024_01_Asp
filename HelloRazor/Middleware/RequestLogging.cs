using Microsoft.AspNetCore.Http.Features;
using System.Text;

namespace HelloRazor.Middleware;

public class RequestLogging
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IConfiguration configuration;
    private bool _isActive = false;
    private int _bodyLogSize = 10;

    public RequestLogging(RequestDelegate next, ILogger<RequestLogging> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        this.configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_isActive = configuration.GetValue<bool>("UseRequestLogging"))
        {
            _bodyLogSize = Math.Max(0, configuration.GetValue<int>("RequestLoggingBodySize", 10));
        }

        if (!_isActive)
        {
            await _next(context);
            return;
        }

        // Log the Request
        var requestInfo = new StringBuilder();
        requestInfo.AppendLine($"Request {context.Request.Method} {context.Request.Path}\n");
        requestInfo.AppendLine($"Client-IP: {context.Connection?.RemoteIpAddress}");

        foreach (var header in context.Request.Headers)
        {
            requestInfo.AppendLine($"Header: {header.Key} Value: {header.Value}");
        }

        if (_bodyLogSize > 0)
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = await reader.ReadToEndAsync();
                requestInfo.AppendLine($"Body: {new string(body.Take(_bodyLogSize).ToArray())}");
                context.Request.Body.Position = 0;
            }
        }
        _logger.LogDebug(requestInfo.ToString());

        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);

            // Log the Response
            var responseInfo = new StringBuilder();
            responseInfo.AppendLine($"Response for {context.Request.Method} {context.Request.Path}\n");
            responseInfo.AppendLine($"Status Code: {context.Response.StatusCode}");
            responseInfo.AppendLine($"Status: {context.Response.HttpContext.Features.Get<IHttpResponseFeature>()?.ReasonPhrase}");

            foreach (var header in context.Response.Headers)
            {
                responseInfo.AppendLine($"Header: {header.Key} Value: {header.Value}");
            }

            if (_bodyLogSize > 0)
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();

                responseInfo.AppendLine($"Body: {new string(text.Take(_bodyLogSize).ToArray())}");
            }
            _logger.LogDebug(responseInfo.ToString());

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
