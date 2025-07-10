using Infrastructure.Services; 
using Prism.Ioc;
using Prism.Modularity;
using SalesOrderApp.Modules.Products.ViewModels;
using Products.VIews; 
namespace SalesOrderApp.Modules.Products
{
    public class ProductsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterForNavigation<ProductListView, ProductListViewModel>();
            containerRegistry.RegisterSingleton<IProductService, ProductService>(); 
        }
    }
}