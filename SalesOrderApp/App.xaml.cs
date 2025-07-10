using Core.Models;
using Costumers.Views;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Products.Views;
using Products.VIews;
using Sales.Views;
using SalesOrderApp.Infrastructure.Data;

using SalesOrderApp.Main.Views;
using SalesOrderApp.Modules.Customers;
using SalesOrderApp.Modules.Customers.ViewModels;
using SalesOrderApp.Modules.Products;
using SalesOrderApp.Modules.Products.ViewModels;
using SalesOrderApp.Modules.Sales;
using SalesOrderApp.Modules.Sales.ViewModels;
using SalesSystem.App.ViewModels;
using System.Windows;

namespace SalesOrderApp
{
    
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var mainView = new MainView();
            mainView.Show();
            return mainView;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate("ContentRegion", "MainView");
            
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CustomersModule>();
            moduleCatalog.AddModule<ProductsModule>();
            moduleCatalog.AddModule<SalesModule>();
        }

        protected override void RegisterTypes(IContainerRegistry container)
        {
       
            container.Register<AppDbContext>(() =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer("Server=DESKTOP-ULPFN7N;Database=SalesOrderDB;Integrated Security=True;TrustServerCertificate=True;");

                return new AppDbContext(optionsBuilder.Options);
            });

            container.Register<ICustomerService, CustomerService>();
            container.Register<IProductService, ProductService>();
            container.Register<ISalesOrderService, SalesOrderService>();
            container.Register<IBaseDialogViewModel, BaseDialogViewModel>();


            container.RegisterForNavigation<CustomerListView, CustomerListViewModel>();
            container.RegisterForNavigation<CustomerDetailView, CustomerDetailViewModel>();
            container.RegisterForNavigation<ProductListView, ProductListViewModel>();
            container.RegisterForNavigation<ProductDetailView, ProductDetailViewModel>();
            container.RegisterForNavigation<ProductDetailView, ProductDetailViewModel>();
            container.RegisterForNavigation<SalesOrderListView, SalesOrderListViewModel>();
            container.RegisterForNavigation<SalesOrderDetailView, SalesOrderDetailViewModel>();
            container.RegisterForNavigation<MainView,MainViewModel > ();
           
        }
    }
    }


