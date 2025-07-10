using Costumers.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SalesOrderApp.Modules.Customers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderApp.Modules.Customers
{
    public class CustomersModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CustomerListView, CustomerListViewModel>();
            containerRegistry.RegisterForNavigation<CustomerDetailView, CustomerDetailViewModel>();
        }
    }
}
