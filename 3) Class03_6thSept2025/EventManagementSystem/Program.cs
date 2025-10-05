using Microsoft.EntityFrameworkCore;
using EventManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EventManagementContext>(options => options.UseSqlite("Data Source=eventmanagement.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EventManagementContext>();
    SeedData.Initialize(context);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=UpcomingEvents}/{id?}");

app.Run();
