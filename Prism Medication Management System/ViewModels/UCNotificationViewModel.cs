using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCNotificationViewModel : BindableBase
	{
        #region Properties
        private MedicineNotification _Notification;
        public MedicineNotification Notification
        {
            get { return _Notification; }
            set { SetProperty(ref _Notification, value); }
        }
        private User _User;
        public User User
        {
            get { return _User; }
            set { SetProperty(ref _User, value); }
        }
        private ObservableCollection<MedicineNotification> _NotificationList;
        public ObservableCollection<MedicineNotification> NotificationList
        {
            get { return _NotificationList; }
            set { SetProperty(ref _NotificationList, value); }
        }
        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                SetProperty(ref _SearchText, value);
                FetchNotificationFromDatabase();
            }
        }
        IEventAggregator _EventAggregator;
        private AppDBContext _AppDbContext;
        private DataService _NotificationDataService;
        public DelegateCommand<MedicineNotification> DeleteNotificationCommand { get; set; }
        #endregion

        #region Construtor
        public UCNotificationViewModel(IEventAggregator eventAggregator, DataService notificationDataService)
        {
            _AppDbContext = new AppDBContext();
            Notification = new MedicineNotification();
            _NotificationDataService = notificationDataService;
            User = new User();
            User = _NotificationDataService.GetSharedUser();
            FetchNotificationFromDatabase();
            DeleteNotificationCommand = new DelegateCommand<MedicineNotification>(DeleteNotification);
            _EventAggregator = eventAggregator;
            _EventAggregator.GetEvent<PubSubEvent<MedicineNotification>>().Subscribe(OnNotificationAdded);
        }
        #endregion

        #region Update Notification Count
        private void OnNotificationAdded(MedicineNotification notification)
        {
            FetchNotificationFromDatabase();
        }
        #endregion

        #region Delete Notification
        private void DeleteNotification(object obj)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to remove notification?", "Remove Notification", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _AppDbContext.notification.Remove((MedicineNotification)obj);
                _AppDbContext.SaveChanges();
                FetchNotificationFromDatabase();
                MessageBox.Show("Removed");
            }
        }
        #endregion

        #region Fetch Notification From Database
        public void FetchNotificationFromDatabase()
        {
            if (String.IsNullOrEmpty(SearchText))
            {
                NotificationList = new ObservableCollection<MedicineNotification>(_AppDbContext.notification
                    .Where(notification => notification.UserId.Equals(User.UserId))
                    .OrderBy(notification => notification.Time));
            }
            else
            {
                string text = SearchText.Trim().ToLower();
                NotificationList = new ObservableCollection<MedicineNotification>(_AppDbContext.notification.
                                 Where(notification => notification.NotificationName
                                 .ToLower()
                                 .Contains(text)));
            }
        }
        #endregion
    }
}
