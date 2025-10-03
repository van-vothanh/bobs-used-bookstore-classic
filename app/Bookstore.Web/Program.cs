using Microsoft.AspNetCore.Authorization;
using Bookstore.Data;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register your services here
    containerBuilder.RegisterModule<DependencyInjectionModule>();
});

// Add authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Authentication/Login";
        options.LogoutPath = "/Authentication/Logout";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
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

// Configure routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Dependency injection module
public class DependencyInjectionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register repositories and services
        builder.RegisterType<Bookstore.Data.Repositories.AddressRepository>()
            .As<Bookstore.Domain.Addresses.IAddressRepository>()
            .InstancePerLifetimeScope();
            
        builder.RegisterType<Bookstore.Data.Repositories.BookRepository>()
            .As<Bookstore.Domain.Books.IBookRepository>()
            .InstancePerLifetimeScope();
            
        builder.RegisterType<Bookstore.Data.Repositories.CustomerRepository>()
            .As<Bookstore.Domain.Customers.ICustomerRepository>()
            .InstancePerLifetimeScope();
            
        builder.RegisterType<Bookstore.Data.Repositories.OfferRepository>()
            .As<Bookstore.Domain.Offers.IOfferRepository>()
            .InstancePerLifetimeScope();
            
        builder.RegisterType<Bookstore.Data.Repositories.OrderRepository>()
            .As<Bookstore.Domain.Orders.IOrderRepository>()
            .InstancePerLifetimeScope();
            
        builder.RegisterType<Bookstore.Data.Repositories.ReferenceDataRepository>()
            .As<Bookstore.Domain.ReferenceData.IReferenceDataRepository>()
            .InstancePerLifetimeScope();
            
        builder.RegisterType<Bookstore.Data.Repositories.ShoppingCartRepository>()
            .As<Bookstore.Domain.Carts.IShoppingCartRepository>()
            .InstancePerLifetimeScope();
    }
}
