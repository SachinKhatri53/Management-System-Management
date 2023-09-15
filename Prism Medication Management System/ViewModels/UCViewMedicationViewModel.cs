using Prism.Events;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using System.Windows;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCViewMedicationViewModel : BindableBase
	{
        #region Properties
        private Medication _NewMedication;
        public Medication NewMedication
        {
            get { return _NewMedication; }
            set { SetProperty(ref _NewMedication, value); }
        }
        private DataService _MedicationDataService;
        #endregion

        #region Constructor
        public UCViewMedicationViewModel(DataService medicationData)
        {
            _MedicationDataService = medicationData;
            NewMedication = new Medication();
            NewMedication = _MedicationDataService.GetSharedMedication();
        }
        #endregion
    }
}