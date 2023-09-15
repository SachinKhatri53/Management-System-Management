using Prism_Medication_Management_System.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Prism_Medication_Management_System.Helpers
{
    public class WindowService : IWindowService
    {
        public void OpenAddWindow(UserControl userControl)
        {
            var addWindow = new AddWindow();
            addWindow.Content = userControl;
            addWindow.ShowDialog();
        }

        public void OpenEditWindow(UserControl userControl)
        {
            var editWindow = new EditWindow();
            editWindow.Content = userControl;
            editWindow.ShowDialog();
        }

        public void OpenViewWindow(UserControl userControl)
        {
            var viewWindow = new ViewWindow();
            viewWindow.Content = userControl;
            viewWindow.ShowDialog();
        }
    }
    public interface IWindowService
    {
        void OpenAddWindow(UserControl userControl);
        void OpenViewWindow(UserControl userControl);
        void OpenEditWindow(UserControl userControl);
    }
}
