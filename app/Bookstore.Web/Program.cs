using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Configure Autofac
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        // TODO: Register Autofac modules here
    });

    // Add services
    builder.Services.AddHttpContextAccessor();
    
    // Configure DbContext
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString))
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
    }

    // Add MVC services
    builder.Services.AddControllersWithViews()
        .AddRazorRuntimeCompilation();

    // Add session support
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    // TODO: Add authentication services

    var app = builder.Build();

    // Configure middleware pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseSession();
    
    // TODO: Add authentication middleware
    // app.UseAuthentication();
    // app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapAreaControllerRoute(
        name: "admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application stopped due to exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
