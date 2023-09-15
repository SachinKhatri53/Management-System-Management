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

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCEditScheduleViewModel : BindableBase
    {
        #region Properties
        private Schedule _Schedule;
        public Schedule Schedule
        {
            get { return _Schedule; }
            set { SetProperty(ref _Schedule, value); }
        }
        private string _SelectedMedication;
        public string SelectedMedication
        {
            get { return _SelectedMedication; }
            set { SetProperty(ref _SelectedMedication, value); }
        }

        private ObservableCollection<string> _HourList;
        public ObservableCollection<string> HourList
        {
            get { return _HourList; }
            set { SetProperty(ref _HourList, value); }
        }

        private string _SelectedHour;
        public string SelectedHour
        {
            get { return _SelectedHour; }
            set { SetProperty(ref _SelectedHour, value); }
        }

        private ObservableCollection<string> _MinuteList;
        public ObservableCollection<string> MinuteList
        {
            get { return _MinuteList; }
            set { SetProperty(ref _MinuteList, value); }
        }

        private string _SelectedMinute;
        public string SelectedMinute
        {
            get { return _SelectedMinute; }
            set { SetProperty(ref _SelectedMinute, value); }
        }

        private ObservableCollection<string> _UnitList;
        public ObservableCollection<string> UnitList
        {
            get { return _UnitList; }
            set { SetProperty(ref _UnitList, value); }
        }
        private string _SelectedUnit;
        public string SelectedUnit
        {
            get { return _SelectedUnit; }
            set { SetProperty(ref _SelectedUnit, value); }
        }
        private DateTime _DateTime;
        public DateTime DateTime
        {
            get { return _DateTime; }
            set { SetProperty(ref _DateTime, value); }
        }
        public AppDBContext AppDbContext { get; set; }
        private DataService _ScheduleDataService;
        private readonly IEventAggregator _EventAggregator;
        public DelegateCommand UpdateScheduleCommand { get; set; }
        #endregion

        #region Constructor
        public UCEditScheduleViewModel(DataService scheduleDataService, IEventAggregator eventAggregator)
        {
            Schedule = new Schedule();
            AppDbContext = new AppDBContext();
            _ScheduleDataService = scheduleDataService;
            Schedule = _ScheduleDataService.GetSharedSchedule();
            GenerateDefaultValuesAndList();
            UpdateScheduleCommand = new DelegateCommand(UpdateSchedule);
            _EventAggregator = eventAggregator;
        }
        #endregion

        #region Update Schedule
        private void UpdateSchedule()
        {
            if(!AreSelectionEmpty(SelectedHour))
            {
                Schedule.Date = $"{DateTime.Year}/{DateTime.Month}/{DateTime.Day}";
                Schedule.Time = $"{SelectedHour}:{SelectedMinute} {SelectedUnit}";
                AppDbContext.schedule.Update(Schedule);
                AppDbContext.SaveChanges();
                _EventAggregator.GetEvent<PubSubEvent<Schedule>>().Publish(Schedule);
                MessageBox.Show("Schedule updated successfully", "Update Schedule", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Windows.OfType<EditWindow>().FirstOrDefault()?.Close();
            } 
        }
        #endregion

        #region Generate Default Values
        private void GenerateDefaultValuesAndList()
        {
            HourList = new ObservableCollection<string>();
            for (int i = 0; i <= 12; i++)
            {
                HourList.Add(i.ToString("D2"));
            }
            SelectedHour = SplitTime(Schedule.Time).hour;
            MinuteList = new ObservableCollection<string>();
            for (int i = 0; i < 60; i++)
            {
                MinuteList.Add(i.ToString("D2"));
            }
            SelectedMinute = SplitTime(Schedule.Time).minute;
            UnitList = new ObservableCollection<string>()
            {
                "AM", "PM"
            };
            SelectedUnit = SplitTime(Schedule.Time).timeUnit;
            DateTime = DateTime.Parse(Schedule.Date);
            SelectedMedication = Schedule.MedicationName;
        }
        #endregion

        #region Split Time
        private (string hour, string minute, string timeUnit) SplitTime(string timeString)
        {
            string[] timeParts = timeString.Split(' ');
            string[] hourMinuteParts = timeParts[0].Split(':');
            return (hourMinuteParts[0], hourMinuteParts[1], timeParts[1]);
        }
        #endregion

        #region Check Empty Selection Validation
        private bool AreSelectionEmpty(string hour)
        {
            if (hour.Equals("00"))
            {
                MessageBox.Show("Please select hour", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else return false;
        }
        #endregion

    }
}
