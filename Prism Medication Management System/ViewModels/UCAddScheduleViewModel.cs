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
    public class UCAddScheduleViewModel : BindableBase
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
        private Medication _Medication;
        public Medication Medication
        {
            get { return _Medication; }
            set { SetProperty(ref _Medication, value); }
        }
        private Dosage _Dosage;
        public Dosage Dosage
        {
            get { return _Dosage; }
            set { SetProperty(ref _Dosage, value); }
        }
        private Report _Report;
        public Report Report
        {
            get { return _Report; }
            set { SetProperty(ref _Report, value); }
        }
        private ObservableCollection<string> _MedicationList;
        public ObservableCollection<string> MedicationList
        {
            get { return _MedicationList; }
            set { SetProperty(ref _MedicationList, value); }
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
        private readonly IEventAggregator _EventAggregator;
        DataService _UserDataService;
        public DelegateCommand AddNewScheduleCommand { get; set; }
        #endregion

        #region Constructor
        public UCAddScheduleViewModel(IEventAggregator eventAggregator, DataService userData)
        {
            AppDbContext = new AppDBContext();
            _EventAggregator = eventAggregator;
            _UserDataService = userData;
            User = new User();
            User = _UserDataService.GetSharedUser();
            MedicationList = new ObservableCollection<string>(AppDbContext.dosage
                .Where(user => user.UserId.Equals(User.UserId))
               .OrderBy(medicine => medicine.MedicationName)
               .Select(medicine => medicine.MedicationName)
               .ToList());
            SelectedMedication = MedicationList.FirstOrDefault();
            GenerateDefaultValuesAndList();
            AddNewScheduleCommand = new DelegateCommand(AddNewSchedule);
        }
        #endregion

        #region Add New Schedule
        private void AddNewSchedule()
        {
            if(!AreSelectionEmpty(SelectedHour))
            {
                Schedule = new Schedule()
                {
                    UserId = User.UserId,
                    MedicationName = SelectedMedication,
                    Time = $"{SelectedHour}:{SelectedMinute} {SelectedUnit}",
                    Date = $"{DateTime.Year}/{DateTime.Month}/{DateTime.Day}"
                };
                AppDbContext.schedule.Add(Schedule);
                Dosage = new Dosage();
                Dosage = FindDosageByMedicationName(Schedule.MedicationName, Schedule.UserId);
                Medication = new Medication();
                Medication = FindMedicationByMedicationName(Schedule.MedicationName, Schedule.UserId);
                AppDbContext.SaveChanges();
                _EventAggregator.GetEvent<PubSubEvent<Schedule>>().Publish(Schedule);
                MessageBox.Show($"New Schedule Added" +
                    $"\nMedication Name: {Schedule.MedicationName}" +
                    $"\nDate: {Schedule.Date}" +
                    $"\nTime: {Schedule.Time}",
                    "Add New Schedule", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Windows.OfType<AddWindow>().FirstOrDefault()?.Close();
            }
        }
        #endregion

        #region Generate Default Values for List
        private void GenerateDefaultValuesAndList()
        {
            HourList = new ObservableCollection<string>();
            for (int i = 0; i <= 12; i++)
            {
                HourList.Add(i.ToString("D2"));
            }
            SelectedHour = HourList.FirstOrDefault();
            MinuteList = new ObservableCollection<string>();
            for (int i = 0; i < 60; i++)
            {
                MinuteList.Add(i.ToString("D2"));
            }
            SelectedMinute = HourList.FirstOrDefault();
            UnitList = new ObservableCollection<string>()
            {
                "AM", "PM"
            };
            SelectedUnit = UnitList.FirstOrDefault();
            DateTime = DateTime.Now;   
        }
        #endregion

        #region Find Dosage By Medication Name
        private Dosage FindDosageByMedicationName(string medicationName, int userId)
        {
            return new ObservableCollection<Dosage>(AppDbContext.dosage
                .Where(dosage => dosage.MedicationName
                .Equals(medicationName) && dosage.UserId.Equals(userId))).FirstOrDefault();
        }
        #endregion

        #region Find Medication By Medication Name
        private Medication FindMedicationByMedicationName(string medicationName, int userId)
        {
            return new ObservableCollection<Medication>(AppDbContext.medication.
                Where(medication => medication.MedicationName.Equals(medicationName) && medication.UserId.Equals(userId))).FirstOrDefault();
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
