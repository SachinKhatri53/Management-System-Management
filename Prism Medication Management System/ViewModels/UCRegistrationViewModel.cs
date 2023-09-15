using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCRegistrationViewModel : BindableBase
    {
        #region Properties
        
        private string _DemoText;
        public string DemoText
        {
            get { return _DemoText; }
            set { SetProperty(ref _DemoText, value); }
        }
        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { SetProperty(ref _IsEnabled, value); }
        }
      
        private User _User;
        public User User
        {
            get { return _User; }
            set { SetProperty(ref _User, value); }
        }
        private bool _IsMale;
        public bool IsMale
        {
            get { return _IsMale; }
            set { SetProperty(ref _IsMale, value); }
        }
        private bool _IsFemale;
        public bool IsFemale
        {
            get { return _IsFemale; }
            set { SetProperty(ref _IsFemale, value); }
        }
        private bool _IsOther;
        public bool IsOther
        {
            get { return _IsOther; }
            set { SetProperty(ref _IsOther, value); }
        }
        private DateTime? _SelectedBirthDate;
        public DateTime? SelectedBirthDate
        {
            get { return _SelectedBirthDate; }
            set { SetProperty(ref _SelectedBirthDate, value); }
        }
        private string _ConfirmPassword;
        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set { SetProperty(ref _ConfirmPassword, value);}
        }
        private string _NewEmail;
        public string NewEmail
        {
            get { return _NewEmail; }
            set { SetProperty(ref _NewEmail, value); }
        }
        private readonly IRegionManager _RegionManager;
        public DelegateCommand RegisterCommand { get; set; }
        public DelegateCommand OpenLoginCommand { get; set; }
        public AppDBContext AppDbContext { get; set; }
        private readonly IEventAggregator _EventAggregator;
        #endregion

        #region Constructor
        public UCRegistrationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            DemoText = "\r\n\"Empower your journey to health with the right tools. " +
                "Sign up for our Medication Management System and take control of your well-being, " +
                "one pill at a time.\"";
            _RegionManager = regionManager;
            _EventAggregator = eventAggregator;
            User = new User();
            
            AppDbContext = new AppDBContext();
            OpenLoginCommand = new DelegateCommand(() => NavigateTo("LoginRegion", "UCLogin"));
            RegisterCommand = new DelegateCommand(RegisterUser).ObservesCanExecute(() => IsEnabled);
        }
        #endregion

        #region Navigation
        // navigate to new user control
        private void NavigateTo(string region, string url)
        {
            if (!String.IsNullOrEmpty(NewEmail))
            {
                var parameters = new NavigationParameters
                {
                    { "Email", NewEmail }
                };
                _RegionManager.RequestNavigate(region, url, parameters);
            }
            else
                _RegionManager.RequestNavigate(region, url);

        }
        #endregion

        #region New User Registration
        // Register new user
        private void RegisterUser()
        {
            if (SelectedBirthDate.HasValue)
            {
                User.BirthDate = SelectedBirthDate.Value.ToString("yyyy-MM-dd");
                User.Age = CalculateAge(SelectedBirthDate);
            }   
            if (IsMale)
                User.Gender = "Male";
            else if (IsFemale)
                User.Gender = "Female";
            else if (IsOther)
                User.Gender = "Other";
            if(!AreFieldsEmpty(User.FirstName, User.LastName, User.Gender, User.BirthDate, User.Email, User.Password, ConfirmPassword))
            {
                if (!CheckPasswordAndConfirmPassword(User.Password, ConfirmPassword))
                    MessageBox.Show("Passwords do not match.", "Validation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (ExistingUser(User.Email).Equals(0))
                    {
                        NewEmail = User.Email;
                        User.Email = User.Email.Trim().ToLower();
                        User.Password = BC.EnhancedHashPassword(User.Password, 13);
                        AppDbContext.user.Add(User);
                        AppDbContext.SaveChanges();
                        _EventAggregator.GetEvent<PubSubEvent<User>>().Publish(User);
                        MessageBoxResult messageBoxResult = MessageBox.Show("New user registered successfully." +
                            $"\nFullname: {User.FirstName} {User.LastName}" +
                            $"\nGender: {User.Gender}" +
                            $"\nDate of Birth: {CalculateAge(SelectedBirthDate)}" +
                            $"\nEmail: {User.Email}" +
                            $"\nDo you want to login?",
                            "Registration Successful", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            ResetFields();
                            NavigateTo("LoginRegion", "UCLogin");
                        }
                        ResetFields();
                    }
                    else
                        MessageBox.Show("User email already exists", "Existing Email Error", MessageBoxButton.OK, MessageBoxImage.Error);   
                }
            }
        }
        #endregion

        #region Calculate Age
        // Calculate age based on date of birth
        public int CalculateAge(DateTime? date)
        {
            DateTime dateTime = DateTime.Now;
            if (dateTime.Month > date.Value.Month)
                return dateTime.Year - date.Value.Year;
            else if(dateTime.Month.Equals(date.Value.Month) && dateTime.Day >= date.Value.Day)
                return dateTime.Year - date.Value.Year;
            return dateTime.Year - date.Value.Year - 1;
        }
        #endregion

        #region Verify Password and Confirm Password
        // Check password and confirm password
        private bool CheckPasswordAndConfirmPassword(string password, string confirmPassword)
        {
            return (password.Equals(confirmPassword));
        }
        #endregion

        #region Fields Validation
        private bool AreFieldsEmpty(string firstName, string lastName, string gender, string birthDate, string email, string password, string confirmPassword)
        {
            if (String.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Firstname cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }  
            else if (String.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Lastname cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
                
            else if (String.IsNullOrWhiteSpace(gender))
            {
                MessageBox.Show("Please select your gender", "Empty Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (String.IsNullOrWhiteSpace(birthDate))
            {
                MessageBox.Show("Please pick your date of birth", "Empty Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (String.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if(!String.IsNullOrWhiteSpace(email))
            {
                Regex emailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
                                        RegexOptions.Compiled | RegexOptions.IgnoreCase);
                bool isEmailValid = Regex.IsMatch(email, $"{emailRegex}");
                if (!isEmailValid)
                {
                    MessageBox.Show("Email is not valid", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }

            else if(String.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            
            else if (String.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Confirm password cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;      
        }
        #endregion

        #region Check Existing Email
        private int ExistingUser(string email)
        {
            return new ObservableCollection<User>(AppDbContext.user.Where(user => user.Email.Equals(email))).Count();
        }
        #endregion
        
        #region Reset Fields
        private void ResetFields()
        {
            User = new User();
            SelectedBirthDate = null;
            ConfirmPassword = string.Empty;
            IsMale = false;
            IsFemale = false;
            IsOther = false;
            IsEnabled = false;
        }
        #endregion

    }
}
