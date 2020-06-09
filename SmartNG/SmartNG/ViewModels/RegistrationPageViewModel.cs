using Java.Lang;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using SmartNG.Views.Pages;
using System.Threading.Tasks;
using System.Net.Http;
using SmartNG.DataProfiles;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using SmartNG.RestAPIClientHandlers;
using SmartNG.Helpers;

namespace SmartNG
{
    class RegistrationPageViewModel : BaseViewModel
    {

        #region PrivateMembers
        private string _fullName { get; set; }
        private string _Email { get; set; }
        private string _password { get; set; }
        private string _confirmPassword { get; set; }
        private int _phoneNumber { get; set; }
        private string _homeAddress { get; set; }
        private bool _isSignUp { get; set; }

        private bool _IsRegSuccessful { get; set; } = false;
        #endregion

        #region PublicMembers

        #region Accessors

        public string FullName
        {

            get => this._fullName;

            set
            {
                this._fullName = value;
                onPropertyChanged();
            }
        }

        public string Email
        {

            get => this._Email;

            set
            {
                this._Email = value;

                var verifyEmail = new EmailValidator(Email);

                //if (verifyEmail.IsValid() == false)
                //{ }
                onPropertyChanged();
            }
        }

        public string Password
        {

            get => this._password;

            set
            {
                this._password = value;
                onPropertyChanged();
            }
        }

        public string ConfirmPassword
        {

            get => this._confirmPassword;

            set
            {
                this._confirmPassword = value;
                onPropertyChanged();
            }
        }

        public int PhoneNumber
        {

            get => this._phoneNumber;

            set
            {
                this._phoneNumber = value;
                onPropertyChanged();
            }
        }

        public string HomeAddress
        {

            get => this._homeAddress;

            set
            {
                this._homeAddress = value;
                onPropertyChanged();
            }
        }

        public bool isSignUpInit
        {
            get => this._isSignUp;

            set
            {
                this._isSignUp = value;
                onPropertyChanged();
            }
        }

        #endregion

        public ICommand UserRegisterCommand { get; private set; }
        /// <summary>
        /// Default Constructor..
        /// </summary>
        public RegistrationPageViewModel()
        {
            UserRegisterCommand = new Command(RegisterUser);
            this.isSignUpInit = false;
        }

        #endregion


        private async void RegisterUser()
        {
            if (this.isSignUpInit == true)
                return;

            await Task.Run(async () =>
            {
                this.isSignUpInit = true;

                RegistrationProfile newUserProfile = new RegistrationProfile()
                {
                    Email = _Email,
                    PassWordHash = _password,
                    Fullname = _fullName,
                    HomeAddress = _homeAddress,
                    PhoneNumber = _phoneNumber.ToString()
                };

                var registeruser = new RegisterUserApi(newUserProfile);

                _IsRegSuccessful = await registeruser.RegisterUser();

                this.isSignUpInit = false;
            });

            if (_IsRegSuccessful)
            {
                _IsRegSuccessful = false;
                await Application.Current.MainPage.DisplayAlert("Registration", "Sucessful", "Login");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }////

            else
            {
                _IsRegSuccessful = false;
                await Application.Current.MainPage.DisplayAlert("test error", "Failed", "Retry");
            } ///
        }

    }
}
