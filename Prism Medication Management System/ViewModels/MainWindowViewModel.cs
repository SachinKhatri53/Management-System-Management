using Prism.Mvvm;

namespace Prism_Medication_Management_System.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Medication Management System";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public MainWindowViewModel()
        {

        }
    }
}
