using Prism.Mvvm;

namespace Prism_Medication_Management_System.ViewModels
{
    public class EditWindowViewModel : BindableBase
	{
        private string _Title = "Medication Management System";
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        public EditWindowViewModel()
        {

        }
	}
}
