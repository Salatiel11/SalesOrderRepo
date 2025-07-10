using Core.Models;
using Infrastructure.Services;
using Infrastructure.ViewModels;
using Prism.Regions;
using System.Threading.Tasks;

namespace SalesOrderApp.Modules.Products.ViewModels
{
    public class ProductDetailViewModel : AbstractDetailViewModel<Product, IProductService>
    {
        private readonly IProductService _service;
        public ProductDetailViewModel(
            IRegionManager regionManager,
            IProductService productService,
            IBaseDialogViewModel baseCrudViewModel)
            : base(regionManager, productService, baseCrudViewModel)
        {
            _service = productService;
            Title = "Product Details";
        }

        protected override async Task<Product> GetItemAsync(int id)
        {
            return await _service.GetAsync(id);
           
        }

        protected override async Task AddItemAsync(Product item)
        {
            await _service.AddAsync(item);
        }

        protected override async Task UpdateItemAsync(Product item)
        {
            await _service.UpdateAsync(item);
        }

        protected override async Task DeleteItemAsync(Product item)
        {
            await _service.DeleteAsync(item.Id);
        }

        protected override bool ValidateItem()
        {
            return !string.IsNullOrWhiteSpace(Item.Price.ToString()) && !string.IsNullOrWhiteSpace(Item.Description) && !string.IsNullOrWhiteSpace(Item.Name);
        }
    }
}