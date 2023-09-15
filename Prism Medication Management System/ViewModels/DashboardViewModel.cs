using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using Prism_Medication_Management_System.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Prism_Medication_Management_System.ViewModels
{
    public class DashboardViewModel : BindableBase
    {
        #region Properties
        private string _title = "Medication Management System";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private ObservableCollection<MedicineNotification> _NotificationList;
        public ObservableCollection<MedicineNotification> NotificationList
        {
            get { return _NotificationList; }
            set { SetProperty(ref _NotificationList, value); }
        }
        private int _NotificationCount;
        public int NotificationCount
        {
            get { return _NotificationCount; }
            set { SetProperty(ref _NotificationCount, value); }
        }

        private Schedule _Schedule;
        public Schedule Schedule
        {
            get { return _Schedule; }
            set { SetProperty(ref _Schedule, value); }
        }
        private ObservableCollection<Schedule> _ScheduleList;
        public ObservableCollection<Schedule> ScheduleList
        {
            get { return _ScheduleList; }
            set { SetProperty(ref _ScheduleList, value); }
        }
        private string _Date;
        public string Date
        {
            get { return _Date; }
            set { SetProperty(ref _Date, value); }
        }
        private DateTime _Time;
        public DateTime Time{
            get{return _Time; }
            set { SetProperty(ref _Time, value); }
        }
        private User _User;
        public User User
        {
            get { return _User; }
            set { SetProperty(ref _User, value); }
        }
     
        private double _SliderValue;
        public double SliderValue
        {
            get { return _SliderValue; }
            set { SetProperty(ref _SliderValue, value); }
        }
        public DelegateCommand LightModeCommand { get; set; }
        public DelegateCommand DarkModeCommand { get; set; }
        public AppDBContext AppDbContext { get; set; }
        private DispatcherTimer timer;
        private DateTime lastSystemTime;
        AppTheme appTheme = new AppTheme();
        public MedicationNotification MedicationNotification { get; set; }
        IEventAggregator _EventAggregator;
        DataService _UserDataService;
        public DelegateCommand LogoutCommand { get; set; }
        #endregion

        #region Constructor
        public DashboardViewModel(IEventAggregator eventAggregator, DataService userDataService)
        {
            AppDbContext = new AppDBContext();
            Date = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            MedicationNotification = new MedicationNotification();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            lastSystemTime = DateTime.Now;
            timer.Start();
            _EventAggregator = eventAggregator;
            _UserDataService = userDataService;
            User = new User();
            User = _UserDataService.GetSharedUser();
            SliderValue = 0;
            NotificationCount = FetchNotificationCountFromDatabase();
            DarkModeCommand = new DelegateCommand(ChangeToDarkTheme);
            LightModeCommand = new DelegateCommand(ChangeToLightTheme);
            LogoutCommand = new DelegateCommand(Logout);
        }
        #endregion

        #region Logout
        private void Logout()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to logout?", "Logout Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(messageBoxResult == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.Windows.OfType<Dashboard>().FirstOrDefault()?.Close();
            }
        }
        #endregion

        #region Change To Dark Theme
        private void ChangeToDarkTheme()
        {
           /* LogoSource = "../Images/logo-dark.png";*/
            appTheme.ChangeTheme(new Uri("Themes/DarkTheme.xaml", UriKind.Relative));
            SliderValue = 1;
        }
        #endregion

        #region Change To Light Theme
        private void ChangeToLightTheme()
        {
          /*  LogoSource = "../Images/logo.png";*/
            appTheme.ChangeTheme(new Uri("Themes/LightTheme.xaml", UriKind.Relative));
            SliderValue = 0;
        }
        #endregion

        #region Fetch Notifcation Count From Database
        public int FetchNotificationCountFromDatabase()
        {
            NotificationList = new ObservableCollection<MedicineNotification>(AppDbContext.notification
                .Where(notification => notification.UserId.Equals(User.UserId)));
            return NotificationList.Count();
        }
        #endregion

        #region Send Notification To Database
        public void SendNotificationToDatabase()
        {
            ScheduleList = new ObservableCollection<Schedule>(AppDbContext.schedule
                .Where(schedule => schedule.UserId.Equals(User.UserId)));
            List<Schedule> matchingSchedules = ScheduleList
                                    .Where(schedule => schedule.Date == Date)
                                    .Where(schedule =>
                                    {
                                        var parts = schedule.Time.Split(' ');
                                        var time = parts[0].Split(':');
                                        return (time[0] == DateTime.Now.ToString("hh") && time[1] == (DateTime.Now.Minute).ToString() && parts[1] == DateTime.Now.ToString("tt", System.Globalization.CultureInfo.InvariantCulture));
                                    }).ToList<Schedule>();
            
            foreach (var matchingSchedule in matchingSchedules)
            {
                MedicineNotification medicineNotification = new MedicineNotification()
                {
                    UserId = User.UserId,
                    NotificationName = matchingSchedule.MedicationName,
                    Time = matchingSchedule.Time
                };
                AppDbContext.notification.Add(medicineNotification);
                AppDbContext.SaveChanges();
                NotificationCount = FetchNotificationCountFromDatabase();
                _EventAggregator.GetEvent<PubSubEvent<MedicineNotification>>().Publish(medicineNotification);
                MedicationNotification.ShowNotification(matchingSchedule.MedicationName, matchingSchedule.Time);
            }
        }
        #endregion

        #region Timer For Notification
        private void TimerTick(object sender, EventArgs e)
        {
            NotificationCount = FetchNotificationCountFromDatabase();
            DateTime currentSystemTime = DateTime.Now;
            if (currentSystemTime.Minute != lastSystemTime.Minute)
            {
                SendNotificationToDatabase();
                lastSystemTime = currentSystemTime;
            }
        }
        #endregion

    }
}
