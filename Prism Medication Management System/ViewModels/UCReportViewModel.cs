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
    public class UCReportViewModel : BindableBase
    {
        #region Properties
        private ObservableCollection<Report> _ReportList;
        public ObservableCollection<Report> ReportList
        {
            get { return _ReportList; }
            set { SetProperty(ref _ReportList, value); }
        }
        private Report _Report;
        public Report Report
        {
            get { return _Report; }
            set { SetProperty(ref _Report, value); }
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
        private ObservableCollection<Dosage> _DosageList;
        public ObservableCollection<Dosage> DosageList
        {
            get { return _DosageList; }
            set { SetProperty(ref _DosageList, value); }
        }
        private ObservableCollection<Schedule> _ScheduleList;
        public ObservableCollection<Schedule> ScheduleList
        {
            get { return _ScheduleList; }
            set { SetProperty(ref _ScheduleList, value); }
        }
        private AppDBContext _AppDbContext;
        private DataService _UserDataService;
        public DelegateCommand<Report> _ViewReportCommand;
        private readonly IWindowService _WindowService;
        IEventAggregator _EventAggregator;
        #endregion

        #region Constructor
        public UCReportViewModel(DataService userDataService, IWindowService windowService, IEventAggregator eventAggregator)
        {
            _AppDbContext = new AppDBContext();
            _WindowService = windowService;
            Report = new Report();
            _UserDataService = userDataService;
            User = new User ();
            User = _UserDataService.GetSharedUser();
            GenerateReport();
            _EventAggregator = eventAggregator;
            _EventAggregator.GetEvent<PubSubEvent<Schedule>>().Subscribe(ReloadReport);
        }

        private void ReloadReport(Schedule schedule)
        {
            GenerateReport();
        }
        #endregion

        #region View Window
        public DelegateCommand<Report> ViewReportCommand =>
            _ViewReportCommand ?? (_ViewReportCommand = new DelegateCommand<Report>(ViewReportWindow));
        private void ViewReportWindow(object obj)
        {
            _UserDataService.SetSharedReport((Report)obj);
            _WindowService.OpenViewWindow(new UCViewReport());
        }
        #endregion

        #region Find Dosage By User Id
        private void FindDosageByUserId(int userId)
        {
            DosageList = new ObservableCollection<Dosage>(_AppDbContext.dosage
                .Where(dosage => dosage.UserId.Equals(userId)));
        }
        #endregion

        #region Find Medication By User Id
        private void FindMedicationByUserId(int userId)
        {
            MedicationList = new ObservableCollection<Medication>(_AppDbContext.medication.
                Where(medication => medication.UserId.Equals(userId)));
        }
        #endregion

        #region Find Schedule By User Id
        private void FindScheduleByUserId(int userId)
        {
            ScheduleList = new ObservableCollection<Schedule>(_AppDbContext.schedule.
                Where(schedule => schedule.UserId.Equals(userId)));
        }
        #endregion

        #region Generate Report
        private void GenerateReport()
        {
            ReportList = new ObservableCollection<Report>();
            FindDosageByUserId(User.UserId);
            FindMedicationByUserId(User.UserId);
            FindScheduleByUserId(User.UserId);
            var query = from medication in MedicationList
                        join dosage in DosageList on medication.MedicationName equals dosage.MedicationName
                        join schedule in ScheduleList on medication.MedicationName equals schedule.MedicationName
                        select new Report
                        {
                            UserId = medication.UserId,
                            MedicationName = medication.MedicationName,
                            MedicationDuration = medication.MedicationDuration,
                            PescribedBy = medication.PescribedBy,
                            IssuedDate = medication.IssuedDate,
                            Amount = dosage.Amount,
                            Mode = dosage.Mode,
                            Frequency = dosage.Frequency,
                            Time = dosage.Time,
                            ScheduleTime = schedule.Time,
                            Date = schedule.Date
                        };
            foreach (var report in query)
            {
                ReportList.Add(report);
            }
        }
        #endregion
    }
}
