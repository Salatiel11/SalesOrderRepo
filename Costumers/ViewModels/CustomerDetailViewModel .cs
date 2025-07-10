using Core.Models;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Regions;

namespace SalesOrderApp.Modules.Customers.ViewModels
{
    public class CustomerDetailViewModel : AbstractDetailViewModel<Customer, ICustomerService>
    {
        private readonly ICustomerService _service;
        public CustomerDetailViewModel(
            IRegionManager regionManager,
            ICustomerService customerService,
            IBaseDialogViewModel baseDialogViewModel)
            : base(regionManager, customerService, baseDialogViewModel)
        {
            _service = customerService;
            Title = "Customer Details";
        }

        protected override async Task<Customer> GetItemAsync(int id)
        {
            return await _service.GetAsync(id);
        }

        protected override async Task AddItemAsync(Customer item)
        {
            await _service.AddAsync(item);
        }

        protected override async Task UpdateItemAsync(Customer item)
        {
            await _service.UpdateAsync(item);
        }

        protected override async Task DeleteItemAsync(Customer item)
        {
            await _service.DeleteAsync(item.Id);
        }

        protected override bool ValidateItem()
        {
            return !string.IsNullOrWhiteSpace(Item.Name) && !string.IsNullOrWhiteSpace(Item.Email);
        }
    }
}