using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Amazon.Rekognition;
using Amazon.S3;
using Bookstore.Data;
using Bookstore.Data.FileServices;
using Bookstore.Data.ImageResizeService;
using Bookstore.Data.ImageValidationServices;
using Bookstore.Data.Repositories;
using Bookstore.Domain;
using Bookstore.Domain.Addresses;
using Bookstore.Domain.Books;
using Bookstore.Domain.Carts;
using Bookstore.Domain.Customers;
using Bookstore.Domain.Offers;
using Bookstore.Domain.Orders;
using Bookstore.Domain.ReferenceData;
using Bookstore.Web;
using BobsBookstoreClassic.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    await ConfigurationSetup.ConfigureConfigurationAsync();
    
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Host.UseNLog();

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddHttpContextAccessor();

    var authService = BookstoreConfiguration.GetSetting("Services/Authentication");
    if (authService == "aws")
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "OpenIdConnect";
        })
        .AddCookie("Cookies")
        .AddOpenIdConnect("OpenIdConnect", options =>
        {
            options.Authority = BookstoreConfiguration.GetSetting("Authentication/Authority");
            options.ClientId = BookstoreConfiguration.GetSetting("Authentication/ClientId");
            options.ClientSecret = BookstoreConfiguration.GetSetting("Authentication/ClientSecret");
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
        });
    }
    else
    {
        builder.Services.AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.LogoutPath = "/Authentication/Logout";
            });
    }

    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterType<BookService>().As<IBookService>();
        containerBuilder.RegisterType<OrderService>().As<IOrderService>();
        containerBuilder.RegisterType<ReferenceDataService>().As<IReferenceDataService>();
        containerBuilder.RegisterType<OfferService>().As<IOfferService>();
        containerBuilder.RegisterType<CustomerService>().As<ICustomerService>();
        containerBuilder.RegisterType<AddressService>().As<IAddressService>();
        containerBuilder.RegisterType<ShoppingCartService>().As<IShoppingCartService>();
        containerBuilder.RegisterType<ImageResizeService>().As<IImageResizeService>();

        var connectionString = BookstoreConfiguration.GetConnectionString("BookstoreDatabaseConnection");
        containerBuilder.Register(c => new ApplicationDbContext(connectionString)).InstancePerLifetimeScope();

        containerBuilder.RegisterType<CustomerRepository>().As<ICustomerRepository>();
        containerBuilder.RegisterType<AddressRepository>().As<IAddressRepository>();
        containerBuilder.RegisterType<BookRepository>().As<IBookRepository>();
        containerBuilder.RegisterType<OfferRepository>().As<IOfferRepository>();
        containerBuilder.RegisterType<ShoppingCartRepository>().As<IShoppingCartRepository>();
        containerBuilder.RegisterType<OrderRepository>().As<IOrderRepository>();
        containerBuilder.RegisterType<ReferenceDataRepository>().As<IReferenceDataRepository>();

        containerBuilder.RegisterGeneric(typeof(PaginatedList<>)).As(typeof(IPaginatedList<>)).InstancePerLifetimeScope();

        if (BookstoreConfiguration.GetSetting("Services/FileService") == "aws")
        {
            containerBuilder.RegisterType<AmazonS3Client>().As<IAmazonS3>();
            containerBuilder.RegisterType<S3FileService>().As<IFileService>();
        }
        else
        {
            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            containerBuilder.RegisterInstance(new LocalFileService(webRootPath)).As<IFileService>();
        }

        if (BookstoreConfiguration.GetSetting("Services/ImageValidationService") == "aws")
        {
            containerBuilder.RegisterType<AmazonRekognitionClient>().As<IAmazonRekognition>();
            containerBuilder.RegisterType<RekognitionImageValidationService>().As<IImageValidationService>();
        }
        else
        {
            containerBuilder.RegisterType<LocalImageValidationService>().As<IImageValidationService>();
        }
    });

    var app = builder.Build();

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

    app.UseSession();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
