using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Prism_Medication_Management_System.Helpers
{
    public class AppTheme
    {
        public void ChangeTheme(Uri themeUri)
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary() { Source = themeUri };
            App.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

    }
}
