using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism_Medication_Management_System.ViewModels;
using Prism_Medication_Management_System.Views;

namespace Prism_Medication_Management_System.Modules
{
    internal class LoginModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("LoginRegion", typeof(UCLogin));
            regionManager.RegisterViewWithRegion("LoginRegion", typeof(UCRegistration));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<UCLogin, UCLoginViewModel>();
            containerRegistry.RegisterForNavigation<UCRegistration, UCRegistrationViewModel>();
        }
    }
}
