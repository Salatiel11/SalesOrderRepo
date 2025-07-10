using Core.Models;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Regions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOrderApp.Modules.Products.ViewModels
{
    public class ProductListViewModel : AbstractListViewModel<Product, IProductService>
    {
        private readonly IProductService _service;
        public ProductListViewModel(
            IProductService productService,
            IRegionManager regionManager,
            IBaseDialogViewModel baseDialogViewModel)
            : base(productService, regionManager, baseDialogViewModel)
        {
            _service = productService;
        }

        protected override string GetDetailViewName()
        {
            return "ProductDetailView"; 
        }

        protected override int GetItemId(Product item)
        {
            return item.Id;
        }

        protected override async Task<IEnumerable<Product>> GetAllItemsAsync()
        {
            return await _service.GetAllAsync();
        }

        protected override async Task DeleteItemAsync(Product item)
        {
            await _service.DeleteAsync(item.Id);
        }

        protected override bool MatchesSearchTerm(Product item, string searchTerm)
        {
            return item.Id.ToString().Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                   item.Description.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}