using Core.Models;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Regions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOrderApp.Modules.Sales.ViewModels
{
    public class SalesOrderListViewModel : AbstractListViewModel<SalesOrder, ISalesOrderService>
    {
        private readonly ISalesOrderService _salesOrderService;
        public SalesOrderListViewModel(
            ISalesOrderService orderService,
            IRegionManager regionManager,
            IBaseDialogViewModel baseDialogViewModel)
            : base(orderService, regionManager, baseDialogViewModel)
        {
            _salesOrderService = orderService;
        }

        protected override string GetDetailViewName()
        {
            return "SalesOrderDetailView";
        }

        protected override int GetItemId(SalesOrder item)
        {
            return item.Id;
        }

        protected override async Task<IEnumerable<SalesOrder>> GetAllItemsAsync()
        {
            return await _salesOrderService.GetAllAsync();
        }

        protected override async Task DeleteItemAsync(SalesOrder item)
        {
            await _salesOrderService.DeleteAsync(item.Id);
        }

        protected override bool MatchesSearchTerm(SalesOrder item, string searchTerm)
        {
            return item.Id.ToString().Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                   item.Customer.Name.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}