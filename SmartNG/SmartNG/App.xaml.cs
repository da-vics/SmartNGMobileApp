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
            pageSetUp();

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

        private void pageSetUp()
        {

            if (Current.Properties.ContainsKey("ApiKey"))
            {
                if (Current.Properties["ApiKey"] != null)
                {
                    MainPage = new ControlPage();
                    return;
                }
            }

            MainPage = new NavigationPage(new MainPage());

        }

    }
}
