using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using Prism_Medication_Management_System.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prism_Medication_Management_System.ViewModels
{
    public class UCDosageViewModel : BindableBase
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
        private ObservableCollection<Dosage> _DosageList;
        public ObservableCollection<Dosage> DosageList
        {
            get { return _DosageList; }
            set { SetProperty(ref _DosageList, value); }
        }
        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                SetProperty(ref _SearchText, value);
                FetchDosageFromDatabase();
            }
        }
        public DelegateCommand<Dosage> DeleteDosageCommand { get; set; }
        private DelegateCommand _AddDosageCommand;
        private DelegateCommand<Dosage> _EditDosageCommand;
        private DelegateCommand<Dosage> _ViewDosageCommand;
        public AppDBContext AppDbContext { get; set; }
        private IRegionManager _RegionManager;
        private readonly IEventAggregator _EventAggregator;
        private readonly IWindowService _WindowService;
        private DataService _DosageDataService;
        #endregion

        #region Constructor
        public UCDosageViewModel(IRegionManager regionManager, IWindowService windowService, DataService dosageData, IEventAggregator eventAggregator)
        {
            Dosage = new Dosage();
            AppDbContext = new AppDBContext();
            _EventAggregator = eventAggregator;
            _RegionManager = regionManager;
            _WindowService = windowService;
            _DosageDataService = dosageData;
            User = new User();
            User = _DosageDataService.GetSharedUser();
            FetchDosageFromDatabase();
            _EventAggregator.GetEvent<PubSubEvent<Dosage>>().Subscribe(UpdateDosageTable);
            DeleteDosageCommand = new DelegateCommand<Dosage>(DeleteDosage);
        }
        #endregion

        #region Update Dosage Table
        private void UpdateDosageTable(Dosage dosage)
        {
            FetchDosageFromDatabase();
        }
        #endregion

        #region Fetch Dosage From Database
        private void FetchDosageFromDatabase()
        {
            if (String.IsNullOrWhiteSpace(SearchText))
            {
                DosageList = new ObservableCollection<Dosage>(AppDbContext.dosage
                    .Where(dosage => dosage.UserId.Equals(User.UserId))
                    .OrderBy(dosage => dosage.MedicationName));
            }
            else
            {
                string text = SearchText.Trim().ToLower();
                DosageList = new ObservableCollection<Dosage>(AppDbContext.dosage
                                 .Where(dosage => dosage.UserId.Equals(User.UserId))
                                 .Where(dosage => dosage.MedicationName
                                 .ToLower()
                                 .Contains(text) || dosage.Time
                                 .ToLower().Contains(text) || dosage.Frequency.ToString().Contains(text)));
            }
        }
        #endregion

        #region Add, Edit and View Window
        public DelegateCommand AddDosageCommand =>
             _AddDosageCommand ?? (_AddDosageCommand = new DelegateCommand(AddDosageWindow));
        public DelegateCommand<Dosage> EditDosageCommand =>
            _EditDosageCommand ?? (_EditDosageCommand = new DelegateCommand<Dosage>(EditDosageWindow));
        public DelegateCommand<Dosage> ViewDosageCommand => 
            _ViewDosageCommand ?? (_ViewDosageCommand = new DelegateCommand<Dosage>(ViewDosageWindow));

        private void AddDosageWindow()
        {
            _WindowService.OpenAddWindow(new UCAddDosage());
        }
        private void EditDosageWindow(object obj)
        {
            _DosageDataService.SetSharedDosage((Dosage)obj);
            _WindowService.OpenEditWindow(new UCEditDosage());
        }
        private void ViewDosageWindow(object obj)
        {
            _DosageDataService.SetSharedDosage((Dosage)obj);
            _WindowService.OpenViewWindow(new UCViewDosage());
        }
        #endregion

        #region Delete Dosage
        private void DeleteDosage(object obj)
        {
            Dosage dosage = (Dosage)obj;
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to remove dosage?", "Remove Dosage", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                AppDbContext.dosage.Remove(dosage);
                var ScheduleList = AppDbContext.schedule
               .Where(schedule => schedule.MedicationName.Equals(((Dosage)obj).MedicationName) && schedule.UserId.Equals(User.UserId));
                AppDbContext.schedule.RemoveRange(ScheduleList);
                _EventAggregator.GetEvent<PubSubEvent<Schedule>>().Publish(ScheduleList.FirstOrDefault());
                AppDbContext.SaveChanges();
                FetchDosageFromDatabase();
                MessageBox.Show("Dosage removed", "Delete Operation", MessageBoxButton.OK, MessageBoxImage.Information);
            }    
        }
        #endregion
    }
}
