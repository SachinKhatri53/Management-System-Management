using Prism.Commands;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism_Medication_Management_System.ViewModels
{
	public class UCViewReportViewModel : BindableBase
	{
        #region Properties
        private Report _Report;
        public Report Report
        {
            get { return _Report; }
            set { SetProperty(ref _Report, value); }
        }
        private DataService _ReportDataService;
        #endregion

        #region Constructor
        public UCViewReportViewModel(DataService reportData)
        {
            _ReportDataService = reportData;
            Report = new Report();
            Report = _ReportDataService.GetSharedReport();
        }
        #endregion
    }
}