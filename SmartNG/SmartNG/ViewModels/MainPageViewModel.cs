
using Android.Util;
using SmartNG.DataProfiles;
using SmartNG.RestAPIClientHandlers;
using SmartNG.Views.Pages;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartNG
{
    public class MainPageViewModel : BaseViewModel
    {

        #region PrivateMembers
        private string _password { get; set; } = string.Empty;
        private string _Email { get; set; } = string.Empty;
        private bool _isLogin { get; set; }

        private string _emailValidation { get; set; } = "required";
        private string _passwordValidation { get; set; } = "required";

        private bool allowLogin { get; set; } = false;

        private bool _IsLogSuccessful { get; set; } = false;
        #endregion

        #region PublicMembers

        #region Accessors
        public string EmailValidation
        {
            get => _emailValidation;

            set
            {
                this._emailValidation = value;
                onPropertyChanged();
            }
        }

        public string PasswordValidation
        {
            get => _passwordValidation;

            set
            {
                this._passwordValidation = value;
                onPropertyChanged();
            }
        }

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

                if (!string.IsNullOrEmpty(_Email))
                {
                    _Email = _Email.Trim();
                }

                onPropertyChanged();

                if (string.IsNullOrEmpty(_Email))
                {
                    EmailValidation = "required";
                    allowLogin = false;
                }

                else
                {
                    EmailValidation = string.Empty;
                    setLoginProp();
                }
            }

        }

        public string Password
        {
            private get => _password;

            set
            {
                this._password = value;

                onPropertyChanged();

                if (string.IsNullOrEmpty(_password))
                {
                    PasswordValidation = "required";
                    allowLogin = false;
                }

                else
                {
                    PasswordValidation = string.Empty;
                    setLoginProp();
                }
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
            if (checkFieldStates())
            {
                await Application.Current.MainPage.DisplayAlert("input error", "one or more input fields not set properly", "retry"); /// test
                return;
            }

            if (this.isLoginInit == true)
                return;

            try
            {
                if (string.IsNullOrEmpty(_Email) || string.IsNullOrEmpty(_password))
                {
                    throw new ArgumentException("Fields cannot be Empty");
                }
            }

            catch (ArgumentException args)
            {
                await Application.Current.MainPage.DisplayAlert("Login Error", args.Message, "Retry");
                return;
            }

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
                Application.Current.MainPage = new ControlPage();
            //  await Application.Current.MainPage.Navigation.PushAsync(new ControlPage());

            else
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Try Again", "Retry");
            }

        }

        private async void UserRegistrationPageSwitch()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new istRegistrationPage());
        }


        private bool checkFieldStates()
        {
            bool check = string.IsNullOrEmpty(_Email)
                & string.IsNullOrEmpty(_password);

            return check;
        }

        private void setLoginProp()
        {
            if (checkFieldStates() == false)
            {
                allowLogin = true;
            }

            else
            {
                allowLogin = false;
            }
        }

    }
}
