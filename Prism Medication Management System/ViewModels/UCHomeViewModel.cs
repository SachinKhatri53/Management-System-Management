using Prism.Events;
using Prism.Mvvm;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using Prism_Medication_Management_System.Modules;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prism_Medication_Management_System.ViewModels
{

    public class UCHomeViewModel : BindableBase
    {
        #region Property
        private User _User;
        public User User
        {
            get { return _User; }
            set { SetProperty(ref _User, value); }
        }
        private Schedule _Schedule;
        public Schedule Schedule
        {
            get { return _Schedule; }
            set { SetProperty(ref _Schedule, value); }
        }
        private Dosage _Dosage;
        public Dosage Dosage
        {
            get { return _Dosage; }
            set { SetProperty(ref _Dosage, value); }
        }
        private int _MedicationCount;
        public int MedicationCount
        {
            get { return _MedicationCount; }
            set { SetProperty(ref _MedicationCount, value); }
        }
        private int _ScheduleCount;
        public int ScheduleCount
        {
            get { return _ScheduleCount; }
            set { SetProperty(ref _ScheduleCount, value); }
        }
        private ObservableCollection<Schedule> _ScheduleList;
        public ObservableCollection<Schedule> ScheduleList
        {
            get { return _ScheduleList; }
            set { SetProperty(ref _ScheduleList, value); }
        }
        private ObservableCollection<Medication> _MedicationList;
        public ObservableCollection<Medication> MedicationList
        {
            get { return _MedicationList; }
            set { SetProperty(ref _MedicationList, value); }
        }
        private ObservableCollection<Schedule> _TodaySchedule;
        public ObservableCollection<Schedule> TodaySchedule
        {
            get { return _TodaySchedule; }
            set { SetProperty(ref _TodaySchedule, value); }
        }
        private ObservableCollection<Dosage> _DosageList;
        public ObservableCollection<Dosage> DosageList
        {
            get { return _DosageList; }
            set { SetProperty(ref _DosageList, value); }
        }
        DataService _SharedUserData;
        AppDBContext _AppDbContext;
        IEventAggregator _EventAggregator;
        #endregion

        #region Constructor
        public UCHomeViewModel(DataService dataService, IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;
            _SharedUserData = dataService;
            User = new User();
            User = _SharedUserData.GetSharedUser();
            MedicationCount = FetchMedicationCount(User.UserId);
            ScheduleCount = FetchScheduleCount(User.UserId);
            _EventAggregator.GetEvent<AddMedicationEvent>().Subscribe(UpdateMedicationCount);
            _EventAggregator.GetEvent<PubSubEvent<Schedule>>().Subscribe(UpdateScheduleCount);
            _EventAggregator.GetEvent<PubSubEvent<Dosage>>().Subscribe(UpdateGraph);
            Schedule = new Schedule();
            Dosage = new Dosage();
            FetchDosage(User.UserId);

            GetTodaySchedule();
        }
        #endregion

        #region Update Graph
        private void UpdateGraph(Dosage dosage)
        {
            FetchDosage(User.UserId);
        }
        #endregion

        #region Update Schedule Count
        private void UpdateScheduleCount(Schedule schedule)
        {
            ScheduleCount = FetchScheduleCount(User.UserId);
            GetTodaySchedule();
        }
        #endregion

        #region Update Medication Count
        private void UpdateMedicationCount(Medication medication)
        {
            MedicationCount = FetchMedicationCount(User.UserId);
        }
        #endregion

        #region Fetch Medication Count
        private int FetchMedicationCount(int userId)
        {
            _AppDbContext = new AppDBContext();
             return new ObservableCollection<Medication>(_AppDbContext.medication
                .Where(mediction => mediction.UserId.Equals(userId))).Count();
        }
        #endregion

        #region Fetch Schedule Count
        private int FetchScheduleCount(int userId)
        {
            _AppDbContext = new AppDBContext();
            return new ObservableCollection<Schedule>(_AppDbContext.schedule
               .Where(schedule => schedule.UserId.Equals(userId))).Count();

        }
        #endregion

        #region Fetch Dosage
        private void FetchDosage(int userId)
        {
            _AppDbContext = new AppDBContext();
            DosageList = new ObservableCollection<Dosage>(_AppDbContext.dosage
               .Where(dosage => dosage.UserId.Equals(userId)));

        }
        #endregion

        #region Get Today's Schedule
        private void GetTodaySchedule()
        {
            string currentDate = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            string currentTime = DateTime.Now.ToString("hh:mm tt");
            _AppDbContext = new AppDBContext();
            TodaySchedule = new ObservableCollection<Schedule>(_AppDbContext.schedule
                .Where(schedule => schedule.UserId.Equals(User.UserId) && schedule.Date.Equals(currentDate)));
        }
        #endregion
    }
}
