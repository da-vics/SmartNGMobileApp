
using SmartNG.DataProfiles;
using SmartNG.RestAPIClientHandlers;
using SmartNG.Views.Pages;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartNG
{
    public class MainPageViewModel : BaseViewModel
    {


        #region PrivateMembers
        private string _password { get; set; }
        private string _Email { get; set; }
        private bool _isLogin { get; set; }

        private bool _IsLogSuccessful { get; set; } = false;
        #endregion

        #region PublicMembers

        #region Accessors

        public bool isLoginInit
        {
            get => this._isLogin;

            set
            {
                this._isLogin = value;

                onPropertyChanged();

            }
        }


        public string Email
        {
            private get => _Email;

            set
            {
                this._Email = value;

                onPropertyChanged();
            }

        }

        public string Password
        {
            private get => _password;

            set
            {
                this._password = value;

                onPropertyChanged();
            }

        }
        #endregion

        public ICommand UserLoginCommand { get; private set; }


        public ICommand RegisterUserCommand { get; private set; }

        public MainPageViewModel()
        {
            UserLoginCommand = new Command(loginUser);
            this.isLoginInit = false;
            RegisterUserCommand = new Command(this.UserRegistrationPageSwitch);
        }


        #endregion

        private async void loginUser()
        {

            if (this.isLoginInit == true)
                return;

            await Task.Run(async () =>
            {
                isLoginInit = true;

                LoginProfile loginProfile = new LoginProfile()
                {
                    Email = _Email,
                    PassWord = _password
                };

                var loginUser = new LoginUserApi(loginProfile);

                _IsLogSuccessful = await loginUser.LoginUser();

                isLoginInit = false;
            });

            if (_IsLogSuccessful)
                await Application.Current.MainPage.Navigation.PushAsync(new ControlPage());

            else
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Try Again", "Retry");
            }

        }

        private async void UserRegistrationPageSwitch()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new RegistrationPage());
        }


    }
}
