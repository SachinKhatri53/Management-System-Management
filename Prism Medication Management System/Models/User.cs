using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism_Medication_Management_System.Models
{
    public class User : BindableBase
    {
        #region Properties
        private int _UserId;
        public int UserId
        {
            get { return _UserId;}
            set { SetProperty(ref _UserId, value);}
        }
        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set { SetProperty(ref _FirstName, value); }
        }
        private string _LastName;
        public string LastName
        {
            get { return _LastName;}
            set { SetProperty(ref _LastName, value); }
        }
        private string _Gender;
        public string Gender
        {
            get { return _Gender; }
            set { SetProperty(ref _Gender, value);}
        }
        private string _BirthDate;
        public string BirthDate
        {
            get { return _BirthDate; }
            set { SetProperty(ref _BirthDate, value); }
        }
        private int _Age;
        public int Age
        {
            get { return _Age; }
            set { SetProperty(ref _Age, value); }
        }
        private string _Email;
        public string Email
        { 
            get { return _Email; }
            set { SetProperty(ref _Email, value); }
        }
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
        #endregion

    }
}
