using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using Prism_Medication_Management_System.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCLoginViewModel : BindableBase, INavigationAware
    {
        #region Properties
        private string _DemoText;
        public string DemoText
        {
            get { return _DemoText; }
            set { SetProperty(ref _DemoText, value); }
        }
        private User _User;
        public User User
        {
            get { return _User; }
            set { SetProperty(ref _User, value); }  
        }
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand OpenRegistrationCommand { get; set; }
        private readonly IRegionManager _RegionManager;
        
        private ObservableCollection<User> _UserList;
        public ObservableCollection<User> UserList
        {
            get { return _UserList; }
            set { SetProperty(ref _UserList, value); }
        }
  
        public AppDBContext AppDbContext { get; set; }
        private readonly IEventAggregator _EventAggregator;
        private DataService _UserDataService;
        #endregion

        #region UCLoginViewModel Constructor
        public UCLoginViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, DataService userDataService)
        {
            DemoText = "\r\n\"Medication can be a powerful ally in the journey to health, " +
                "but wisdom lies in understanding its potential and taking every precaution " +
                "to ensure its safe and effective use.\"";
            User = new User();
            _EventAggregator = eventAggregator;
            _EventAggregator.GetEvent<PubSubEvent<User>>().Subscribe(UpdateUserTable);
            AppDbContext = new AppDBContext();
            FetchUserFromDatabse();
            _RegionManager = regionManager;
            LoginCommand = new DelegateCommand(LoginUser);
            OpenRegistrationCommand = new DelegateCommand(() => NavigateTo("LoginRegion", "UCRegistration"));
            _EventAggregator = eventAggregator;
            _UserDataService = userDataService;
        }
        #endregion

        #region Navigation
        private void NavigateTo(string region, string url)
        {
            _RegionManager.RequestNavigate(region, url);
            
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Email"))
            {
                User.Email = navigationContext.Parameters["Email"] as string;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion

        #region Fetch User From Database
        private void FetchUserFromDatabse()
        {
            UserList = new ObservableCollection<User>(AppDbContext.user);
        }
        #endregion

        #region Update User List
        private void UpdateUserTable(User user)
        {
            FetchUserFromDatabse();
        }
        #endregion

        #region Login User
        // Login user
        private void LoginUser()
        {
            if (IsEmailAndPasswordEmpty(User.Email, Password))
                MessageBox.Show("Fields cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (FindUserByEmail(User.Email.Trim().ToLower()) == null)
                MessageBox.Show("Email does not exist", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (BC.EnhancedVerify(Password, FindUserByEmail(User.Email.Trim().ToLower()).Password))
                {
                    _UserDataService.SetSharedUser(FindUserByEmail(User.Email.Trim().ToLower()));
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                    Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
                }
                else
                    MessageBox.Show("Invalid login credentials", "Credentials Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        #endregion

        #region Find User By Email
        public User FindUserByEmail(string email)
        {
            return UserList.FirstOrDefault<User>(user => user.Email.Equals(email));
        }
        #endregion

        #region Empty Fields Validations
        private bool IsEmailAndPasswordEmpty(string email, string password)
        {
            return (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password));
        }
        #endregion
    }
}
