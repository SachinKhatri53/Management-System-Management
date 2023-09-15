using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_Medication_Management_System.Models
{
    public class Schedule : BindableBase
    {
        private int _ScheduleId;
        public int ScheduleId
        {
            get { return _ScheduleId; }
            set { SetProperty(ref _ScheduleId, value); }
        }
        private int _UserId;
        public int UserId
        {
            get { return _UserId; }
            set { SetProperty(ref _UserId, value); }
        }
        private string _MedicationName;
        public string MedicationName
        {
            get { return _MedicationName; }
            set { SetProperty(ref _MedicationName, value); }
        }
        private string _Time;
        public string Time
        {
            get { return _Time; }
            set { SetProperty(ref _Time, value);}
        }
        private string _Date;

        public string Date
        {
            get { return _Date; }
            set { SetProperty(ref _Date, value); }
        }

    }
}
