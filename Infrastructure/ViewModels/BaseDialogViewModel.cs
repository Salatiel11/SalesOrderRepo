using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public interface IBaseDialogViewModel
    {
        
        public void ShowValidationDialog(string message);
        public void ShowConfirmationDialog(string message, Action onConfirm, Action onCancel = null);


    }
    public class BaseDialogViewModel : IBaseDialogViewModel
    {

        public void ShowValidationDialog(string message)
        {

            System.Windows.MessageBox.Show($"Validation error:\n{message}", "Failed Validation", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }

        public void ShowConfirmationDialog(string message, Action onConfirm, Action onCancel = null)
        {
            var result = System.Windows.MessageBox.Show(
                $"Confirmation:\n{message}",
                "Confirmation needed",
                System.Windows.MessageBoxButton.OKCancel,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.OK)
            {
                onConfirm?.Invoke();
            }
            else if (result == System.Windows.MessageBoxResult.Cancel)
            {
                onCancel?.Invoke();
            }
        }
    }
}