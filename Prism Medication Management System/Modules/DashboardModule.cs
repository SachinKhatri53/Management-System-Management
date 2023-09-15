using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism_Medication_Management_System.ViewModels;
using Prism_Medication_Management_System.Views;

namespace Prism_Medication_Management_System.Modules
{

    internal class DashboardModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("HomeRegion", typeof(UCHome));
            regionManager.RegisterViewWithRegion("HomeRegion", typeof(UCMedication));
            regionManager.RegisterViewWithRegion("HomeRegion", typeof(UCDosage));
            regionManager.RegisterViewWithRegion("AddRegion", typeof(UCAddDosage));
            regionManager.RegisterViewWithRegion("HomeRegion", typeof(UCSchedule));
            regionManager.RegisterViewWithRegion("HomeRegion", typeof(UCReport));
            regionManager.RegisterViewWithRegion("HomeRegion", typeof(UCNotification));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<UCHome, UCHomeViewModel>();
            containerRegistry.RegisterForNavigation<UCMedication, UCMedicationViewModel>();
            containerRegistry.RegisterForNavigation<UCDosage, UCDosageViewModel>();
            containerRegistry.RegisterForNavigation<UCSchedule, UCScheduleViewModel>();
            containerRegistry.RegisterForNavigation<UCReport, UCReportViewModel>();
            containerRegistry.RegisterForNavigation<UCNotification, UCNotificationViewModel>();

        }
    }
}
