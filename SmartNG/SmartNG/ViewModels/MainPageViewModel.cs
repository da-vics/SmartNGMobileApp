
using SmartNG.Views.Pages;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartNG
{
    public class MainPageViewModel : BaseViewModel
    {

        public ICommand UserSignInCommand { get; private set; }

        public ICommand RegisterUserCommand { get; private set; }

        public MainPageViewModel()
        {
            UserSignInCommand = new Command(this.UserPageSwitch);
            RegisterUserCommand = new Command(this.UserRegistrationPageSwitch);
        }

        private async void UserPageSwitch()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        private async void UserRegistrationPageSwitch()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage());
        }


    }
}
