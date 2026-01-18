public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private const string LogFilePath = "api_logs.txt";

    public LoggingMiddleware(RequestDelegate next) { _next = next; }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/chatHub") ||
            context.Request.Path.StartsWithSegments("/graphql"))
        {
            await _next(context);
            return;
        }

        var logEntry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {context.Request.Method} {context.Request.Path} " +
                       $"Query: {context.Request.QueryString} " +
                       $"IP: {context.Connection.RemoteIpAddress} " +
                       $"Status: {context.Response.StatusCode}\n";

        await File.AppendAllTextAsync(LogFilePath, logEntry); 

        await _next(context);
    }
}