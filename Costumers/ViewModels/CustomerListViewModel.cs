using Core.Models;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Regions;
using SalesOrderApp.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesOrderApp.Modules.Customers.ViewModels
{
    public class CustomerListViewModel : AbstractListViewModel<Customer, ICustomerService>
    {
        private readonly ICustomerService _service;
        public CustomerListViewModel(ICustomerService customerService, IRegionManager regionManager, IBaseDialogViewModel baseDialogViewModel)
            : base(customerService, regionManager, baseDialogViewModel)
        {
            _service = customerService;
        }

        protected override string GetDetailViewName()
        {
            return "CustomerDetailView"; 
        }

        protected override int GetItemId(Customer item)
        {
            return item.Id; 
        }

        protected override async Task<IEnumerable<Customer>> GetAllItemsAsync()
        {
            return await _service.GetAllAsync(); 
        }

        protected override async Task DeleteItemAsync(Customer item)
        {
            await _service.DeleteAsync(item.Id); 
        }

        protected override bool MatchesSearchTerm(Customer item, string searchTerm)
        {
            
            return item.Name.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                   item.Email.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase);
        }
        
    }
}