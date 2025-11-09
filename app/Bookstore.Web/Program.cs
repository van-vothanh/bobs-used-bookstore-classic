using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bookstore.Data;
using BobsBookstoreClassic.Data;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Configure NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Configure Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register services with Autofac
});

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Initialize configuration
BookstoreConfiguration.Initialize(builder.Configuration);

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "OpenIdConnect";
})
.AddCookie("Cookies")
.AddOpenIdConnect("OpenIdConnect", options =>
{
    // Configure OpenID Connect options from configuration
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
