// SalesSystem.App/ViewModels/MainViewModel.cs
using Prism.Commands;
using Prism.Regions; // Assuming BaseViewModel is here
using SalesOrderApp.Main;
using SalesOrderApp.Main.ViewModels;
using System.Diagnostics;

namespace SalesSystem.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _title = "Sales Order App";
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
           
        }

        private void Navigate(string viewName)
        {

            _regionManager.RequestNavigate("MainContentRegion", viewName);

        }
       
    }
}
