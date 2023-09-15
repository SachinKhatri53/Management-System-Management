using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_Medication_Management_System.Models
{
    public class Dosage : BindableBase
    {
        private int _DosageId;
        public int DosageId
        {
            get { return _DosageId; }
            set { SetProperty(ref _DosageId, value); }
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
    }
}