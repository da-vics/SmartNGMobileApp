
using Android.Util;
using SmartNG.DataProfiles;
using SmartNG.RestAPIClientHandlers;
using SmartNG.RestAPIClientHandlers.CustomExceptions;
using SmartNG.Views.Pages;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG
{
    public class MainPageViewModel : BaseViewModel
    {

        #region PrivateMembers

        private bool _isPageActive { get; set; } = false;

        private string _password { get; set; } = string.Empty;
        private string _Email { get; set; } = string.Empty;
        private bool _isLogin { get; set; }

        private string _emailValidation { get; set; } = "required";
        private string _passwordValidation { get; set; } = "required";

        private bool _hasEmailError { get; set; } = true;
        private bool _hasPasswordError { get; set; } = true;

        private bool? _IsLogSuccessful { get; set; } = false;
        #endregion

        #region PublicMembers

        #region Accessors


        public bool IsPageActive
        {
            get => _isPageActive;

            set
            {
                _isPageActive = value;
                onPropertyChanged();

            }
        }

        public bool HasEmailError
        {
            get => _hasEmailError;

            set
            {
                _hasEmailError = value;
                onPropertyChanged();
            }
        }

        public bool HasPasswordError
        {

            get => _hasPasswordError;

            set
            {
                _hasPasswordError = value;
                onPropertyChanged();
            }
        }

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
                    HasEmailError = true;
                }

                else
                {
                    EmailValidation = string.Empty;
                    HasEmailError = false;
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
                    HasPasswordError = true;
                }

                else
                {
                    PasswordValidation = string.Empty;
                    HasPasswordError = false;
                }
            }

        }
        #endregion

        public ICommand UserLoginCommand { get; private set; }

        public ICommand RegisterUserCommand { get; private set; }

        public MainPageViewModel()
        {
            UserLoginCommand = new Command(async () => await loginUser());
            this.isLoginInit = false;
            RegisterUserCommand = new Command(async () => await UserRegistrationPageSwitch());
        }

        #endregion

        private async Task loginUser()
        {
            if (!checkFieldStates())
            {
                await Application.Current.MainPage.DisplayAlert("input error", "one or more input fields not set properly", "retry"); /// test
                return;
            }

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

                try
                {
                    _IsLogSuccessful = await loginUser.LoginUser();
                }

                catch (SmartNgHttpException args)
                {
                    _IsLogSuccessful = null;
                    Console.WriteLine(args.Message);
                }


                isLoginInit = false;
            });


            if (_IsLogSuccessful == true)
            {
                Application.Current.MainPage = new ControlPage();
                await Application.Current.MainPage.Navigation.PopToRootAsync(false);
            }

            else if (_IsLogSuccessful == null)
            {

                await Application.Current.MainPage.DisplayAlert("User Not Found", "Login Failed", "Retry");

            }

            else
            {
                await Application.Current.MainPage.DisplayAlert("Service Error", "Login Failed", "Retry");
            }

        }

        private async Task UserRegistrationPageSwitch()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new istRegistrationPage());
        }


        private bool checkFieldStates()
        {
            bool check = string.IsNullOrEmpty(EmailValidation)
                & string.IsNullOrEmpty(PasswordValidation);

            return check;
        }

        public async Task startupInitChecks()
        {
            IsPageActive = false;

            await Task.Run(async () =>
            {
                await Task.Delay(1000);
                IsPageActive = true;

            });
        }

    }
}
