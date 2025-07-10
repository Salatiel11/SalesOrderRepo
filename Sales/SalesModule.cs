using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Sales.Views;

namespace SalesOrderApp.Modules.Sales
{
    public class SalesModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public SalesModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
           
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(SalesOrderListView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterForNavigation<SalesOrderDetailView, SalesOrderDetailViewModel>();

           
        }
    }
}