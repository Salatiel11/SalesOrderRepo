using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public abstract class AbstractListViewModel<TModel, TService> : BindableBase, INavigationAware
        where TModel : class, new()
        where TService : class
    {
        private readonly TService _service;
        private readonly IRegionManager _regionManager;
        private readonly IBaseDialogViewModel _baseDialogViewModel;

        public DelegateCommand<TModel> EditCommand { get; }
        public DelegateCommand<TModel> AddCommand { get; }
        public DelegateCommand<TModel> DeleteCommand { get; }
        private ObservableCollection<TModel> _items = new ObservableCollection<TModel>();
        private string _searchTerm = string.Empty;
        private IEnumerable<TModel> _entities = Enumerable.Empty<TModel>();

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                SetProperty(ref _searchTerm, value);
                ExecuteSearch();
            }
        }

        public ObservableCollection<TModel> Items
        {
            get => _items;
            private set => SetProperty(ref _items, value);
        }

        protected AbstractListViewModel(TService service, IRegionManager regionManager, IBaseDialogViewModel baseCrudViewModel)
        {
            _service = service;
            _regionManager = regionManager;
            _baseDialogViewModel = baseCrudViewModel;

            EditCommand = new DelegateCommand<TModel>(OnEditItem);
            AddCommand = new DelegateCommand<TModel>(OnAddItem);
            DeleteCommand = new DelegateCommand<TModel>(OnDeleteItem);
        }

        private void OnEditItem(TModel item)
        {
            if (item == null)
            {
                _baseDialogViewModel.ShowValidationDialog("Please select an item to edit.");
                return;
            }

            var parameters = new NavigationParameters
            {
                { "mode", "edit" },
                { "itemId", GetItemId(item) }
            };

            _regionManager.RequestNavigate("MainContentRegion", GetDetailViewName(), parameters);
        }

        private void OnAddItem(TModel item)
        {
            var parameters = new NavigationParameters
            {
                { "mode", "add" },
            };

            _regionManager.RequestNavigate("MainContentRegion", GetDetailViewName(), parameters);
        }

        private void OnDeleteItem(TModel item)
        {
            var parameters = new NavigationParameters
            {
                { "mode", "delete" },
                { "itemId", GetItemId(item) }
            };

            _regionManager.RequestNavigate("MainContentRegion", GetDetailViewName(), parameters);
        }

        private async void LoadItems()
        {
            var items = await GetAllItemsAsync();
            _entities = items;
            Items = new ObservableCollection<TModel>(items);
        }

        private void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Items = new ObservableCollection<TModel>(_entities);
                return;
            }

            Items = new ObservableCollection<TModel>(
                _entities.Where(item => MatchesSearchTerm(item, SearchTerm)));
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadItems();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        protected abstract string GetDetailViewName();
        protected abstract int GetItemId(TModel item);
        protected abstract Task<IEnumerable<TModel>> GetAllItemsAsync();
        protected abstract Task DeleteItemAsync(TModel item);
        protected abstract bool MatchesSearchTerm(TModel item, string searchTerm);
    }
}