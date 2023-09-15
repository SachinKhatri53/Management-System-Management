using Prism.Commands;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCViewScheduleViewModel : BindableBase
    {
        #region Properties
        private Schedule _Schedule;
        public Schedule Schedule
        {
            get { return _Schedule; }
            set { SetProperty(ref _Schedule, value); }
        }
        private DataService _ScheduleDataService;
        #endregion

        #region Constructor
        public UCViewScheduleViewModel(DataService dataService)
        {
            _ScheduleDataService = dataService;
            Schedule = new Schedule();
            Schedule = _ScheduleDataService.GetSharedSchedule();
        }
        #endregion
    }
}