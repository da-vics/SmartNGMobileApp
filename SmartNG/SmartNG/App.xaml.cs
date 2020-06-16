using SmartNG.Views.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjcyMjM1QDMxMzgyZTMxMmUzMFJUY0w4Z0RzUGlTeXV1VmNwdDJ5cEE5NzROanRHRThMTmtEcnJoU1pDMXc9");
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            /// MainPage = new ControlPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
