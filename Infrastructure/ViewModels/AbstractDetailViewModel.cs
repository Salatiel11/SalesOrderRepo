using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public abstract class AbstractDetailViewModel<TModel, TService> : BindableBase, INavigationAware
        where TModel : class, new()
        where TService : class
    {
        private readonly IRegionManager _regionManager;
        private readonly TService _service;
        private readonly IBaseDialogViewModel _baseDialogViewModel;

        private TModel _item = new();
        public string _mode = "add";
        private bool _isFieldsEnabled = true;

        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public bool IsFieldsEnabled
        {
            get => _isFieldsEnabled;
            set => SetProperty(ref _isFieldsEnabled, value);
        }

        public TModel Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected AbstractDetailViewModel(IRegionManager regionManager, TService service, IBaseDialogViewModel baseCRUDViewModel)
        {
            _regionManager = regionManager;
            _service = service;
            _baseDialogViewModel = baseCRUDViewModel;

            ConfirmCommand = new DelegateCommand(async () => await OnConfirm());
            CancelCommand = new DelegateCommand(OnCancel);
        }

        public virtual async Task OnConfirm()
        {
            if (_mode == "add")
            {
                if (!ValidateItem())
                {
                    _baseDialogViewModel.ShowValidationDialog("Validation failed.");
                    return;
                }
                await AddItemAsync(Item);
            }
            else if (_mode == "edit")
            {
                if (!ValidateItem())
                {
                    _baseDialogViewModel.ShowValidationDialog("Validation failed.");
                    return;
                }
                await UpdateItemAsync(Item);
            }
            else if (_mode == "delete")
            {
                _baseDialogViewModel.ShowConfirmationDialog(
                    $"Are you sure you want to delete this item?",
                    async () =>
                    {
                        await DeleteItemAsync(Item);
                        NavigateBack();
                    },
                    OnCancel);
                return;
            }
            NavigateBack();
        }

        private void OnCancel()
        {
            NavigateBack();
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            _mode = navigationContext.Parameters["mode"] as string;
            var hasId = navigationContext.Parameters.TryGetValue("itemId", out int itemId);

            if (_mode == "edit" && hasId)
            {
                Title = "Edit Item";
                LoadItem(itemId);
                IsFieldsEnabled = true;
            }
            else if (_mode == "add")
            {
                Title = "Add Item";
                Item = new TModel();
                IsFieldsEnabled = true;
            }
            else if (_mode == "delete" && hasId)
            {
                Title = "Delete Item";
                LoadItem(itemId);
                IsFieldsEnabled = false;
            }
        }

        private async void LoadItem(int id)
        {
            Item = await GetItemAsync(id) ?? new TModel();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        protected abstract Task<TModel> GetItemAsync(int id);
        protected abstract Task AddItemAsync(TModel item);
        protected abstract Task UpdateItemAsync(TModel item);
        protected abstract Task DeleteItemAsync(TModel item);
        protected abstract bool ValidateItem();
        protected virtual void NavigateBack()
        {
            _regionManager.Regions["MainContentRegion"].NavigationService.Journal.GoBack();
        }
    }
}