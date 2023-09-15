using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using Prism_Medication_Management_System.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCAddDosageViewModel : BindableBase
    {
        #region Properties
        private Dosage _Dosage;
        public Dosage Dosage
        {
            get { return _Dosage; }
            set { SetProperty(ref _Dosage, value); }
        }

        private User _User;
        public User User
        {
            get { return _User; }
            set { SetProperty(ref _User, value); }
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
        private ObservableCollection<int> _FrequencyList;
        public ObservableCollection<int> FrequencyList
        {
            get { return _FrequencyList; }
            set { SetProperty(ref _FrequencyList, value); }
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
                SetProperty(ref _SelectedMode , value);
            }
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
        public AppDBContext AppDbContext { get; set; }
        private readonly IEventAggregator _EventAggregator;
        public DelegateCommand AddNewDosageCommand { get; set; }
        DataService _UserDataService;
        #endregion

        #region Constructor
        public UCAddDosageViewModel(IEventAggregator eventAggregator, DataService userData)
        {
            _EventAggregator = eventAggregator;
            AppDbContext = new AppDBContext();
            AddNewDosageCommand = new DelegateCommand(AddNewDosage);
            GenerateFrequency();
            _UserDataService = userData;
            User = new User();
            User = _UserDataService.GetSharedUser();
            MedicationList = new ObservableCollection<string>(AppDbContext.medication.Where(user => user.UserId.Equals(User.UserId))
                .OrderBy(medicine => medicine.MedicationName)
                .Select(medicine => medicine.MedicationName).ToList());
            SelectedMedication = MedicationList.FirstOrDefault();
            SelectedAmountUnit = AmountUnit.FirstOrDefault();
            SelectedFrequency = FrequencyList.FirstOrDefault();
            SelectedTime = Time.FirstOrDefault();
            SelectedMode = ModeList.FirstOrDefault();
            
        }
        #endregion

        #region Add New Dosage
        private void AddNewDosage()
        {
            if(!AreFieldsEmpty(Amount, SelectedFrequency))
            {
                Dosage = new Dosage()
                {
                    UserId = User.UserId,
                    MedicationName = SelectedMedication,
                    Amount = DosageAmount(Amount, SelectedAmountUnit),
                    Mode = SelectedMode,
                    Frequency = SelectedFrequency,
                    Time = SelectedTime
                };
                AppDbContext.dosage.Add(Dosage);
                AppDbContext.SaveChanges();
                _EventAggregator.GetEvent<PubSubEvent<Dosage>>().Publish(Dosage);
                MessageBox.Show($"Dosage added successfully" +
                    $"\nMedication Name: {SelectedMedication}" +
                    $"\nAmount: {Amount} {SelectedAmountUnit}" +
                    $"\nMode: {SelectedMode}" +
                    $"\nFrequency: {SelectedFrequency}" +
                    $"\nTime: {SelectedTime}", "Add New Dosage", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Windows.OfType<AddWindow>().FirstOrDefault()?.Close();
            }
        }
        #endregion

        #region Generate Frequency
        private void GenerateFrequency()
        {
            FrequencyList = new ObservableCollection<int>();
            for (int i = 0; i<=10; i++)
            {
                FrequencyList.Add(i);
            }
            AmountUnit = new ObservableCollection<string>()
            {
                "none", "mg", "drops", "ml"
            };
            ModeList = new ObservableCollection<string>()
            {
                "Opthalmic", "Oral", "Otic", "Topical"
            };
            Time = new ObservableCollection<string>()
            {
                "Before Meal", "After Meal"
            };
        }
        #endregion

        #region Concatinate Dosage Amount
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
