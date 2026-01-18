using ChatAppApi.Models;
using ChatAppApi.Hubs;
using Microsoft.EntityFrameworkCore;
using ChatAppApi.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>();
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>() 
    .AddType<UserType>()                      
    .AddFiltering()                             
    .AddSorting()                             
    .AddProjections();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Ensure DB is up

    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Name = "Alice", Address = "123 St" },
            new User { Name = "Bob", Address = "456 Ave" }
        );
        db.SaveChanges();
    }
}

// In app section:
app.MapHub<ChatHub>("/chatHub");
app.MapGraphQL(); // Endpoint: /graphql
app.UseHttpsRedirection();
app.UseMiddleware<LoggingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
