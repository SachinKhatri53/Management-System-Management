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
    public class UCMedicationViewModel : BindableBase
    {
        #region Property
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
            set { SetProperty(ref _User, value); }
        }
        private ObservableCollection<Medication> _MedicationList;
        public ObservableCollection<Medication> MedicationList
        {
            get { return _MedicationList; }
            set { SetProperty(ref _MedicationList, value); }
        }
        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set { SetProperty(ref _SearchText, value);
                FetchMedicationFromDatabase();
            }
        }
        public AppDBContext AppDbContext { get; set; }
        private readonly IEventAggregator _EventAggregator;
        public DelegateCommand<Medication> _EditMedicationCommand;
        public DelegateCommand<Medication> DeleteMedicationCommand { get; set; }
        public DelegateCommand<Medication> _ViewMedicationCommand;
        private DelegateCommand _AddMedicationCommand;
        private DataService _MedicationDataService;
        private readonly IWindowService _WindowService;
        #endregion

        #region Constructor
        public UCMedicationViewModel(IEventAggregator eventAggregator, DataService medicationData, IWindowService windowService)
        {
            _EventAggregator = eventAggregator;
            _WindowService = windowService;
            AppDbContext = new AppDBContext();
            _MedicationDataService = medicationData;
            User = new User();
            User = _MedicationDataService.GetSharedUser();
            FetchMedicationFromDatabase();
            Medication = new Medication();
            _EventAggregator.GetEvent<AddMedicationEvent>().Subscribe(UpdateMedicationTable);
            DeleteMedicationCommand = new DelegateCommand<Medication>(DeleteMedication);
        }
        #endregion

        #region Property For Add, Edit and View Window
        public DelegateCommand AddMedicationCommand =>
        _AddMedicationCommand ?? (_AddMedicationCommand = new DelegateCommand(AddMedicationWindow));

        public DelegateCommand<Medication> EditMedicationCommand =>
            _EditMedicationCommand ?? (_EditMedicationCommand = new DelegateCommand<Medication>(EditMedicationWindow));

        public DelegateCommand<Medication> ViewMedicationCommand =>
        _ViewMedicationCommand ?? (_ViewMedicationCommand = new DelegateCommand<Medication>(ViewMedicationWindow));
        #endregion

        #region Add Medication Window
        private void AddMedicationWindow()
        {
            _WindowService.OpenAddWindow(new UCAddMedication());
        }
        #endregion

        #region Edit Medication Window
        private void EditMedicationWindow(object obj)
        {
            _MedicationDataService.SetSharedMedication((Medication)obj);
            _WindowService.OpenEditWindow(new UCEditMedication());
        }
        #endregion

        #region View Medication Window
        private void ViewMedicationWindow(object obj)
        {
            _MedicationDataService.SetSharedMedication((Medication)obj);
            _WindowService.OpenViewWindow(new UCViewMedication());
        }
        #endregion

        #region Update Medication Table
        private void UpdateMedicationTable(Medication medication)
        {
            FetchMedicationFromDatabase();
        }
        #endregion

        #region Fetch Medication From Database
        private void FetchMedicationFromDatabase()
        {
            if (String.IsNullOrEmpty(SearchText))
            {
                MedicationList = new ObservableCollection<Medication>(AppDbContext.medication
                    .Where(medication => medication.UserId.Equals(User.UserId))
                    .OrderBy(medication => medication.MedicationName));
            }
            else
            {
                string text = SearchText.Trim().ToLower();
                MedicationList = new ObservableCollection<Medication>(AppDbContext.medication
                                 .Where(medication => medication.UserId.Equals(User.UserId))
                                 .Where(medication => medication.MedicationName
                                 .ToLower()
                                 .Contains(text) || medication.PescribedBy.ToLower().Contains(text)));
            }
        }
        #endregion

        #region Delete Medication
        private void DeleteMedication(object obj)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to remove medication?", "Remove Medication", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                AppDbContext.medication.Remove((Medication)obj);
                if(GetDosageByMedicationName(((Medication)obj).MedicationName, User.UserId) != null)
                    AppDbContext.dosage.Remove(GetDosageByMedicationName(((Medication)obj).MedicationName, User.UserId));
                var ScheduleList = AppDbContext.schedule
               .Where(schedule => schedule.MedicationName.Equals(((Medication)obj).MedicationName) && schedule.UserId.Equals(User.UserId));
                if (GetDosageByMedicationName(((Medication)obj).MedicationName, User.UserId) != null && !ScheduleList.Count().Equals(0))
                    AppDbContext.schedule.RemoveRange(ScheduleList);
                AppDbContext.SaveChanges();
                _EventAggregator.GetEvent<AddMedicationEvent>().Publish((Medication)obj);
                _EventAggregator.GetEvent<PubSubEvent<Dosage>>().Publish(GetDosageByMedicationName(((Medication)obj).MedicationName, User.UserId));
                _EventAggregator.GetEvent<PubSubEvent<Schedule>>().Publish(ScheduleList.FirstOrDefault());
                FetchMedicationFromDatabase();
                MessageBox.Show("Medication removed", "Delete Operation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region Get Dosage By Medication Name
        private Dosage GetDosageByMedicationName(string medicineName, int userId)
        {
            return new ObservableCollection<Dosage>(AppDbContext.dosage
                .Where(dosage => dosage.MedicationName.Equals(medicineName) && dosage.UserId.Equals(userId))).FirstOrDefault();
        }
        #endregion
    }
}
