using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_Medication_Management_System.Models
{
    public class MedicineNotification : BindableBase
    {
        private int _NotificationId;
        [Key]
        public int NotificationId
        {
            get { return _NotificationId; }
            set { SetProperty(ref _NotificationId, value); }
        }
        private int _UserId;
        public int UserId
        {
            get { return _UserId; }
            set { SetProperty(ref _UserId, value); }
        }
        private string _NotificationName;
        public string NotificationName
        {
            get { return _NotificationName; }
            set { SetProperty(ref _NotificationName, value); }
        }
        private string _Time;
        public string Time
        {
            get { return _Time; }
            set { SetProperty(ref _Time, value); }
        }
    }
}
