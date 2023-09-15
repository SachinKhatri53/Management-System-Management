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
    public class UCEditMedicationViewModel : BindableBase
	{
        #region Properties
        private Medication _NewMedication;
        public Medication NewMedication
        {
            get { return _NewMedication; }
            set { SetProperty(ref _NewMedication, value); }
        }
       
        private ObservableCollection<int> _DurationLengthList;
        public ObservableCollection<int> DurationLengthList
        {
            get { return _DurationLengthList; }
            set
            {
                SetProperty(ref _DurationLengthList, value);
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
            }
        }
        private string _SelectedDuration;
        public string SelectedDuration
        {
            get { return _SelectedDuration; }
            set { SetProperty(ref _SelectedDuration, value); }
        }

        private DateTime _IssuedDate;
        public DateTime IssuedDate
        {
            get { return _IssuedDate; }
            set { SetProperty(ref _IssuedDate, value); }
        }
        public DelegateCommand UpdateMedicationCommand { get; set; }
        AppDBContext AppDbContext { get; set; }
        private readonly IEventAggregator _EventAggregator;
        private DataService _MedicationDataService;
        #endregion

        #region Constructor
        public UCEditMedicationViewModel(IEventAggregator eventAggregator, DataService medicationData)
        {
            _MedicationDataService = medicationData;
            NewMedication = new Medication();
            NewMedication = _MedicationDataService.GetSharedMedication();
            IssuedDate = DateTime.Parse(NewMedication.IssuedDate);
            SplitLengthAndDuration(NewMedication.MedicationDuration);
            DurationLengthList = new ObservableCollection<int>();
            DurationList = new ObservableCollection<string>()
            {
                "days", "months"
            };
            GenerateDuration();
            AppDbContext = new AppDBContext();
            UpdateMedicationCommand = new DelegateCommand(UpdateMedication);
        }
        #endregion

        #region Update Medication
        private void UpdateMedication()
        {
            if(!AreFieldsEmpty(NewMedication.MedicationName, SelectedDurationLength, NewMedication.PescribedBy))
            {
                NewMedication.MedicationDuration = SelectedDurationLength + " " + SelectedDuration;
                NewMedication.IssuedDate = $"{IssuedDate.Year}/{IssuedDate.Month}/{IssuedDate.Day}";
                AppDbContext.medication.Update(NewMedication);
                AppDbContext.SaveChanges();
                MessageBox.Show("Medication updated successfully", "Update Medication", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Windows.OfType<EditWindow>().FirstOrDefault()?.Close();
            }
        }
        #endregion

        #region Split Duration Length And Duration Time
        private void SplitLengthAndDuration(string date)
        {
            string[] dateParts = date.Split(' ');
            SelectedDurationLength = int.Parse(dateParts[0]);
            SelectedDuration = dateParts[1];
        }
        #endregion

        #region Generate Duration
        private void GenerateDuration()
        {
            for (int i = 0; i <= 32; i++)
            {
                DurationLengthList.Add(i);
            }
        }
        #endregion

        #region Check Empty Fields Validation
        private bool AreFieldsEmpty(string medicationName, int duration, string prescriber)
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
            else if (String.IsNullOrWhiteSpace(prescriber))
            {
                MessageBox.Show("Prescriber name cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else return false;
        }
        #endregion
    }
}
