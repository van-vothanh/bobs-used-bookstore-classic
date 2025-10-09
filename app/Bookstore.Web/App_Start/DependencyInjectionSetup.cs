using System.IO;
using System.Reflection;
using Amazon.Rekognition;
using Amazon.S3;
using Autofac;
using BobsBookstoreClassic.Data;
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

namespace Bookstore.Web
{
    public static class DependencyInjectionSetup
    {
        public static void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterType<BookService>().As<IBookService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<ReferenceDataService>().As<IReferenceDataService>().InstancePerLifetimeScope();
            builder.RegisterType<OfferService>().As<IOfferService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<ShoppingCartService>().As<IShoppingCartService>().InstancePerLifetimeScope();
            builder.RegisterType<ImageResizeService>().As<IImageResizeService>().InstancePerLifetimeScope();

            var connectionString = BookstoreConfiguration.GetConnectionString("BookstoreDatabaseConnection");
            builder.RegisterType<ApplicationDbContext>().WithParameter("connectionString", connectionString).InstancePerLifetimeScope();

            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AddressRepository>().As<IAddressRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BookRepository>().As<IBookRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OfferRepository>().As<IOfferRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ShoppingCartRepository>().As<IShoppingCartRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ReferenceDataRepository>().As<IReferenceDataRepository>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(PaginatedList<>)).As(typeof(IPaginatedList<>)).InstancePerLifetimeScope();

            if (BookstoreConfiguration.GetSetting("Services/FileService") == "aws")
            {
                builder.RegisterType<AmazonS3Client>().As<IAmazonS3>().SingleInstance();
                builder.RegisterType<S3FileService>().As<IFileService>().InstancePerLifetimeScope();
            }
            else
            {
                var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                builder.RegisterInstance(new LocalFileService(webRootPath)).As<IFileService>();
            }

            if (BookstoreConfiguration.GetSetting("Services/ImageValidationService") == "aws")
            {
                builder.RegisterType<AmazonRekognitionClient>().As<IAmazonRekognition>().SingleInstance();
                builder.RegisterType<RekognitionImageValidationService>().As<IImageValidationService>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<LocalImageValidationService>().As<IImageValidationService>().InstancePerLifetimeScope();
            }
        }
    }
}
