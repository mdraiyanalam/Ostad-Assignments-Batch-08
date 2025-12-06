using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// TODO: Add rate limiting services here
// Configure a fixed window policy named "weatherPolicy"
// - Window: 15 seconds
// - Permit limit: 5 requests
// - No queue (reject immediately)
// - Set rejection status to 429 with custom JSON response
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync("{\"error\": \"Rate limit exceeded. Try again soon.\"}", token);
    };

    options.AddPolicy("weatherPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(15),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

var app = builder.Build();

// TODO: Enable rate limiting middleware here
app.UseRateLimiter();

// Sample endpoint: Returns fake weather data
app.MapGet("/weather", () =>
{
    var weather = new
    {
        Temperature = 72,
        Condition = "Sunny"
    };
    return Results.Ok(weather);
})
// TODO: Apply the rate limiting policy to this endpoint
.RequireRateLimiting("weatherPolicy");

app.Run();