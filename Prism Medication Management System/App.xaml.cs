using Microsoft.EntityFrameworkCore.Infrastructure;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism_Medication_Management_System.Helpers;
using Prism_Medication_Management_System.Models;
using Prism_Medication_Management_System.Modules;
using Prism_Medication_Management_System.ViewModels;
using Prism_Medication_Management_System.Views;
using System.Windows;

namespace Prism_Medication_Management_System
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DatabaseFacade facade = new DatabaseFacade(new AppDBContext());
            facade.EnsureCreated();
        }
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<DataService>();
            containerRegistry.Register<IWindowService, WindowService>();

        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<LoginModule>();
            moduleCatalog.AddModule<DashboardModule>();
        }
       
    }
}
