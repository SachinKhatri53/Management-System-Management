using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using Prism_Medication_Management_System.Modules;
using Prism_Medication_Management_System.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCAddMedicationViewModel : BindableBase
	{
        #region Properties
        public AppDBContext AppDbContext { get; set; }  
        private Medication _Medication;
        public Medication Medication
        {
            get { return _Medication; }
            set { SetProperty(ref _Medication, value); }
        }
        private User _User;
        public User User
        {
            get { return _User; }
            set { SetProperty(ref _User, value);}
        }
        private ObservableCollection<int> _DurationLengthList;
        public ObservableCollection<int> DurationLengthList
        {
            get { return _DurationLengthList; }
            set
            {
                SetProperty(ref _DurationLengthList, value);
                SelectedDurationLength = _DurationLengthList.FirstOrDefault();
            }
        }
        private int _SelectedDurationLength;
        public int SelectedDurationLength
        {
            get { return _SelectedDurationLength; }
            set { SetProperty(ref _SelectedDurationLength, value); }
        }
        private ObservableCollection<string> _DurationList;
        public ObservableCollection<string> DurationList
        {
            get { return _DurationList; }
            set
            {
                SetProperty(ref _DurationList, value);
                SelectedDuration = _DurationList.FirstOrDefault();
            }
        }
        private string _SelectedDuration;
        public string SelectedDuration
        {
            get { return _SelectedDuration; }
            set { SetProperty(ref _SelectedDuration, value); }
        }
        private DateTime _DateTime;
        public DateTime DateTime
        {
            get { return _DateTime; }
            set { SetProperty(ref _DateTime, value); }  
        }
        private readonly IEventAggregator _EventAggregator;
        private DataService _UserDataService;
        public DelegateCommand AddNewMedicationCommand { get; set; }
        #endregion

        #region Constructor
        public UCAddMedicationViewModel(IEventAggregator eventAggregator, DataService userData)
        {
            _EventAggregator = eventAggregator;
            AppDbContext = new AppDBContext();
            DurationLengthList = new ObservableCollection<int>();
            DurationList = new ObservableCollection<string>()
            {
                "days", "months"
            };
            GenerateDuration();
            Medication = new Medication();
            DateTime = DateTime.Now;
            AddNewMedicationCommand = new DelegateCommand(AddNewMedication);
            _UserDataService = userData;
            User = new User();
            User = _UserDataService.GetSharedUser();
        }
        #endregion

        #region Add New Medication
        private void AddNewMedication()
        {
            if (!AreFieldsEmpty(Medication.MedicationName, SelectedDurationLength, Medication.PescribedBy))
            {
                Medication.MedicationDuration = SelectedDurationLength + " " + SelectedDuration;
                Medication.IssuedDate = $"{DateTime.Year}/{DateTime.Month}/{DateTime.Day}";
                Medication.UserId = User.UserId;
                AppDbContext.medication.Add(Medication);
                AppDbContext.SaveChanges();
                _EventAggregator.GetEvent<AddMedicationEvent>().Publish(Medication);
                MessageBox.Show($"New medication added successfully" +
                    $"\nMedicine Name: {Medication.MedicationName}" +
                    $"\nDuration: {SelectedDurationLength} {SelectedDuration}" +
                    $"\nPrescribed By: {Medication.PescribedBy}" +
                    $"\nIssued Date: {Medication.IssuedDate}", "Add New Medication", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Windows.OfType<AddWindow>().FirstOrDefault()?.Close();
            }
            
        }
        #endregion

        #region Generate Duration
        private void GenerateDuration()
        {
            for (int i = 0; i <=32; i++)
            {
                DurationLengthList.Add(i);
            }
        }
        #endregion

        #region Empty Check Validation
        private bool AreFieldsEmpty(string medicationName, int duration, string prescribedBy)
        {
            if (String.IsNullOrWhiteSpace(medicationName))
            {
                MessageBox.Show("Medication name cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (duration.Equals(0))
            {
                MessageBox.Show("Please select a duration", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (String.IsNullOrWhiteSpace(prescribedBy))
            {
                MessageBox.Show("Prescriber name cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else return false;
        }
        #endregion
    }
}
