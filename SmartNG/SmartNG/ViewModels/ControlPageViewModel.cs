using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartNG
{
    public class ControlPageViewModel : BaseViewModel
    {

        public ICommand AddNewService { get; private set; }

        public ICommand UserLogout { get; private set; }

        public ControlPageViewModel()
        {
            AddNewService = new Command(AddServiceComponent);
            UserLogout = new Command(LogOutuser);
        }

        private void AddServiceComponent()
        {

        }


        private async void LogOutuser()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
            await Application.Current.MainPage.Navigation.PopToRootAsync(false);
        }

    }
}
