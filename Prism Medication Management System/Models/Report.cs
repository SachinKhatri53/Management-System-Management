using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_Medication_Management_System.Models
{
    public class Report : BindableBase
    {
        private int _ReportId;
        public int ReportId
        {
            get { return _ReportId; }
            set { SetProperty(ref _ReportId, value); }
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
        private string _MedicationDuration;
        public string MedicationDuration
        {
            get { return _MedicationDuration; }
            set { SetProperty(ref _MedicationDuration, value); }
        }
        private string _PescribedBy;
        public string PescribedBy
        {
            get { return _PescribedBy; }
            set { SetProperty(ref _PescribedBy, value); }
        }
        private string _IssuedDate;
        public string IssuedDate
        {
            get { return _IssuedDate; }
            set { SetProperty(ref _IssuedDate, value); }
        }
        private string _Amount;
        public string Amount
        {
            get { return _Amount; }
            set { SetProperty(ref _Amount, value); }
        }
        private string _Mode;
        public string Mode
        {
            get { return _Mode; }
            set { SetProperty(ref _Mode, value); }
        }
        private int _Frequency;
        public int Frequency
        {
            get { return _Frequency; }
            set { SetProperty(ref _Frequency, value); }
        }
        private string _Time;
        public string Time
        {
            get { return _Time; }
            set { SetProperty(ref _Time, value); }
        }
        private string _ScheduleTime;
        public string ScheduleTime
        {
            get { return _ScheduleTime; }
            set { SetProperty(ref _ScheduleTime, value); }
        }
        private string _Date;

        public string Date
        {
            get { return _Date; }
            set { SetProperty(ref _Date, value); }
        }
    }
}
