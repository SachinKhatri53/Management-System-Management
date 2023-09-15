using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_Medication_Management_System.Models
{
    public class Medication : BindableBase
    {
        private int _MedicationId;
        public int MedicationId
        {
            get { return _MedicationId; }
            set { SetProperty(ref _MedicationId, value); }
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

    }
}
