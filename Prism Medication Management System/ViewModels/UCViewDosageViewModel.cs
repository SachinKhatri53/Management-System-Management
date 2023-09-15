using Prism.Commands;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCViewDosageViewModel : BindableBase
    {
        #region Properties
        private Dosage _Dosage;
        public Dosage Dosage
        {
            get { return _Dosage; }
            set { SetProperty(ref _Dosage, value); }
        }
        private DataService _DosageDataService;
        #endregion

        #region Constructor
        public UCViewDosageViewModel(DataService dosageData)
        {
            Dosage = new Dosage();
            _DosageDataService = dosageData;
            Dosage = _DosageDataService.GetSharedDosage();
        }
        #endregion
    }
}