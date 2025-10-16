using Microsoft.EntityFrameworkCore;
using HealthCalendar.DAL;
using Serilog;
using Serilog.Events;
//using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
// var ConnectionString = builder.Configuration.GetConnectionString("DatabaseContextConnection") ?? throw new InvalidOperationException("Connection string 'DatabaseContextConnection' not found.");

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionString:DatabaseContextConnection"]);
});

// builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddScoped<IAvailableDateRepo, AvailableDateRepo>();
builder.Services.AddScoped<IPersonellRepo, PersonellRepo>();
builder.Services.AddScoped<IPatientRepo, PatientRepo>();
builder.Services.AddScoped<IEventRepo, EventRepo>();

// builder.Services.AddRazorPages();
// builder.Services.AddSession();

var loggerConfirmation = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

loggerConfirmation.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) && 
                                      e.Level == LogEventLevel.Information && 
                                      e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfirmation.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    DatabaseInit.Seed(app);
}

app.UseStaticFiles();
//app.UseSession();
//app.UseAuthentication();
//app.UseAuthorization
app.MapDefaultControllerRoute();
//app.MapRazorPages();
app.Run();
