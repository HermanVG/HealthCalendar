using Microsoft.EntityFrameworkCore;
using HealthCalendar.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionString:DatabaseContextConnection"]);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DatabaseInit.Seed(app);
}

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.Run();
