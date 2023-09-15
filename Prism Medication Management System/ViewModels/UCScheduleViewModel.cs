using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using Prism_Medication_Management_System.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCScheduleViewModel : BindableBase
    {
        #region Properties
        private Schedule _Schedule;
        public Schedule Schedule
        {
            get { return _Schedule; }
            set { SetProperty(ref _Schedule, value); }
        }
        private User _User;
        public User User
        {
            get { return _User; }
            set { SetProperty(ref _User, value); }
        }
        private ObservableCollection<Schedule> _ScheduleList;
        public ObservableCollection<Schedule> ScheduleList
        {
            get { return _ScheduleList; }
            set { SetProperty(ref _ScheduleList, value); }
        }

        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                SetProperty(ref _SearchText, value);
                FetchScheduleFromDatabase();
            }
        }

        private DelegateCommand<Schedule> _EditScheduleCommand;
        public DelegateCommand<Schedule> DeleteScheduleCommand { get; set; }
        private DelegateCommand<Schedule> _ViewScheduleCommand;
        private DelegateCommand _AddScheduleCommand;
        private readonly IWindowService _WindowService;
        public AppDBContext AppDbContext { get; set; }
        private readonly IEventAggregator _EventAggregator;
        private DataService _ScheduleDataService;
        #endregion

        #region Constructor
        public UCScheduleViewModel(IWindowService windowService, IEventAggregator eventAggregator, DataService scheduleDataService)
        {
            Schedule = new Schedule();
            AppDbContext = new AppDBContext();
            _WindowService = windowService;
            _EventAggregator = eventAggregator;
            _EventAggregator.GetEvent<PubSubEvent<Schedule>>().Subscribe(UpdateScheduleTable);
            _ScheduleDataService = scheduleDataService;
            User = new User();
            User = _ScheduleDataService.GetSharedUser();
            FetchScheduleFromDatabase();
            DeleteScheduleCommand = new DelegateCommand<Schedule>(DeleteSchedule);
        }
        #endregion

        #region Update Schedule Table
        private void UpdateScheduleTable(Schedule schedule)
        {
            FetchScheduleFromDatabase();
        }
        #endregion

        #region Fecth Schedule From Database
        private void FetchScheduleFromDatabase()
        {
            if (String.IsNullOrWhiteSpace(SearchText))
            {
                ScheduleList = new ObservableCollection<Schedule>(AppDbContext.schedule
                    .Where(schedule => schedule.UserId.Equals(User.UserId)));
            }
            else
            {
                string text = SearchText.Trim().ToLower();
                ScheduleList = new ObservableCollection<Schedule>(AppDbContext.schedule
                                 .Where(schedule => schedule.UserId.Equals(User.UserId))
                                 .Where(schedule => schedule.MedicationName
                                 .ToLower()
                                 .Contains(text)));
            }
        }
        #endregion

        #region Open Add, Edit and View Window
        public DelegateCommand AddScheduleCommand =>
            _AddScheduleCommand ?? (_AddScheduleCommand = new DelegateCommand(AddScheduleWindow));
        public DelegateCommand<Schedule> ViewScheduleCommand =>
            _ViewScheduleCommand ?? (_ViewScheduleCommand = new DelegateCommand<Schedule>(ViewScheduleWindow));
        public DelegateCommand<Schedule> EditScheduleCommand =>
            _EditScheduleCommand ?? (_EditScheduleCommand = new DelegateCommand<Schedule>(EditScheduleWindow));
        
        private void AddScheduleWindow()
        {
            _WindowService.OpenAddWindow(new UCAddSchedule());
        }
        private void EditScheduleWindow(object obj)
        {
            _ScheduleDataService.SetSharedSchedule((Schedule)obj);
            _WindowService.OpenEditWindow(new UCEditSchedule());
        }
        private void ViewScheduleWindow(object obj)
        {
            _ScheduleDataService.SetSharedSchedule((Schedule)obj);
            _WindowService.OpenViewWindow(new UCViewSchedule());
        }
        #endregion

        #region Delete Schedule
        private void DeleteSchedule(object obj)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to remove schedule?", "Remove Schedule", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                AppDbContext.schedule.Remove((Schedule)obj);
                AppDbContext.SaveChanges();
                _EventAggregator.GetEvent<PubSubEvent<Schedule>>().Publish((Schedule)obj);
                FetchScheduleFromDatabase();
                MessageBox.Show("Removed");
               
            }
        }
        #endregion
    }
}
