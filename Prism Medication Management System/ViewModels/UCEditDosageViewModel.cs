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
    public class UCEditDosageViewModel : BindableBase
    {
        #region Properties
        private Dosage _Dosage;
        public Dosage Dosage
        {
            get { return _Dosage; }
            set { SetProperty(ref _Dosage, value); }
        }
    
        private string _MedicationName;
        public string MedicationName
        {
            get { return _MedicationName; }
            set { SetProperty(ref _MedicationName, value); }
        }

        private ObservableCollection<int> _FrequencyList;
        public ObservableCollection<int> FrequencyList
        {
            get { return _FrequencyList; }
            set { SetProperty(ref _FrequencyList, value); }
        }
        private int _SelectedFrequency;
        public int SelectedFrequency
        {
            get { return _SelectedFrequency; }
            set
            {
                SetProperty(ref _SelectedFrequency, value);
            }
        }

        private ObservableCollection<string> _ModeList;
        public ObservableCollection<string> ModeList
        {
            get { return _ModeList; }
            set { SetProperty(ref _ModeList, value); }
        }
        private string _SelectedMode;
        public string SelectedMode
        {
            get { return _SelectedMode; }
            set
            {
                SetProperty(ref _SelectedMode, value);
            }
        }
       
        private int _Amount;
        public int Amount
        {
            get { return _Amount; }
            set { SetProperty(ref _Amount, value); }
        }

        private ObservableCollection<string> _AmountUnit;
        public ObservableCollection<string> AmountUnit
        {
            get { return _AmountUnit; }
            set { SetProperty(ref _AmountUnit, value); }
        }

        private string _SelectedAmountUnit;
        public string SelectedAmountUnit
        {
            get { return _SelectedAmountUnit; }
            set { SetProperty(ref _SelectedAmountUnit, value); }

        }

        private ObservableCollection<string> _Time;
        public ObservableCollection<string> Time
        {
            get { return _Time; }
            set { SetProperty(ref _Time, value); }
        }

        private string _SelectedTime;
        public string SelectedTime
        {
            get { return _SelectedTime; }
            set { SetProperty(ref _SelectedTime, value); }
        }
        private DataService _DosageDataService;
        public DelegateCommand UpdateDosageCommand { get; set; }
        private AppDBContext _AppDbContext;
        IEventAggregator _EventAggregator;
        #endregion

        #region Constructor
        public UCEditDosageViewModel(DataService dosageData, IEventAggregator eventAggregator)
        {
            _DosageDataService = dosageData;
            Dosage = new Dosage();
            Dosage = _DosageDataService.GetSharedDosage();
            GenerateFrequency();
            UpdateDosageCommand = new DelegateCommand(UpdateDosage);
            _EventAggregator = eventAggregator;
        }
        #endregion

        #region Update Dosage
        private void UpdateDosage()
        {
            if(!AreFieldsEmpty(Amount, SelectedFrequency))
            {
                Dosage.Amount = DosageAmount(Amount, SelectedAmountUnit);
                Dosage.Mode = SelectedMode;
                Dosage.Frequency = SelectedFrequency;
                Dosage.Time = SelectedTime;
                _AppDbContext = new AppDBContext();
                _AppDbContext.dosage.Update(Dosage);
                _AppDbContext.SaveChanges();
                _EventAggregator.GetEvent<PubSubEvent<Dosage>>().Publish(Dosage);
                MessageBox.Show($"Dosage updated successfully", "Update Dosage", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Windows.OfType<EditWindow>().FirstOrDefault()?.Close();
            }
              
        }
        #endregion

        #region Generate Frequency
        private void GenerateFrequency()
        {
            FrequencyList = new ObservableCollection<int>();
            for (int i = 0; i <= 10; i++)
            {
                FrequencyList.Add(i);
            }
            SelectedFrequency = Dosage.Frequency;
            AmountUnit = new ObservableCollection<string>()
            {
                "none", "mg", "drops", "ml"
            };
            ModeList = new ObservableCollection<string>()
            {
                "Opthalmic", "Oral", "Otic", "Topical"
            };
            SelectedMode = Dosage.Mode;
            Time = new ObservableCollection<string>()
            {
                "Before Meal", "After Meal"
            };
            SelectedTime = Dosage.Time;
            SplitAmount(Dosage.Amount);
        }
        #endregion

        #region Split Amount
        private void SplitAmount(string amount)
        {
            string[] amountParts = amount.Split(' ');
            Amount = int.Parse(amountParts[0]);
            if (amountParts[1].Equals("n/a"))
            {
                SelectedAmountUnit = "none";
            }
            else
            {
                SelectedAmountUnit = amountParts[1];
            }
        }
        #endregion

        #region Dosage Amount
        private string DosageAmount(int amount, string unit)
        {
            if (unit.Equals("none"))
                return $"{amount} n/a";
            return $"{amount} {unit}";
        }
        #endregion

        #region Check Empty Fields Validation
        private bool AreFieldsEmpty(int amount, int frequency)
        {
            if (amount.Equals(0))
            {
                MessageBox.Show("Amount cannot be empty", "Empty Field Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (frequency.Equals(0))
            {
                MessageBox.Show("Please select frequency", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else return false;
        }
        #endregion
    }
}
