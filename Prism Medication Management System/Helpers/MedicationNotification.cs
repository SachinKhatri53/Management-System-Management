using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_Medication_Management_System.Helpers
{
    public class MedicationNotification
    {
        private readonly NotificationManager _notificationManager = new NotificationManager();
        public void ShowNotification(string? medicine, string? time)
        {
            var content = new NotificationContent
            {
                Title = "Time to take medicine",
                Message = $"Medicine: {medicine} \n Time: {time}",
                Type = NotificationType.Information
            };
            _notificationManager.Show(content);
        }

    }
}
