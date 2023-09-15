using Prism_Medication_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_Medication_Management_System.Helpers
{
    public class DataService
    {
        private Medication _SharedMedication;
        public Medication GetSharedMedication()
        {
            return _SharedMedication;
        }
        public void SetSharedMedication(Medication data)
        {
            _SharedMedication = data;
        }
        private Dosage _SharedDosage;
        public Dosage GetSharedDosage()
        {
            return _SharedDosage;
        }
        public void SetSharedDosage(Dosage data)
        {
            _SharedDosage = data;
        }
        private Schedule _SharedSchedule;
        public Schedule GetSharedSchedule()
        {
            return _SharedSchedule;
        }
        public void SetSharedSchedule(Schedule data)
        {
            _SharedSchedule = data;
        }
        private int _SharedNotificationCount;
        public int GetSharedNotificationCount()
        {
            return _SharedNotificationCount;
        }
        public void SetSharedNotificationCount(int data)
        {
            _SharedNotificationCount = data;
        }
        private User _SharedUser;
        public User GetSharedUser()
        {
            return _SharedUser;
        }
        public void SetSharedUser(User data)
        {
            _SharedUser = data;
        }
        private Report _SharedReport;
        public Report GetSharedReport()
        {
            return _SharedReport;
        }
        public void SetSharedReport(Report data)
        {
            _SharedReport = data;
        }
    }
}
