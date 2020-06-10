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
using Android.Renderscripts;

namespace SmartNG
{
    class RegistrationPageViewModel : BaseViewModel
    {

        #region PrivateMembers
        private string _phoneNumber { get; set; } = string.Empty;
        private string _homeAddress { get; set; } = string.Empty;
        private bool _isSignUp { get; set; } = false;

        private string _phoneNumberValidation { get; set; } = "required";
        private string _addressValidation { get; set; } = "required";

        private bool _allowRegister { get; set; } = false;

        private bool _IsRegSuccessful { get; set; } = false;
        #endregion

        #region PublicMembers

        #region Accessors

        public string PhoneNumberValidation
        {
            get => _phoneNumberValidation;

            set
            {
                this._phoneNumberValidation = value;
                onPropertyChanged();
            }
        }

        public string AddressValidation
        {
            get => _addressValidation;

            set
            {
                this._addressValidation = value;
                onPropertyChanged();
            }
        }

        public string PhoneNumber
        {

            get => this._phoneNumber;

            set
            {
                this._phoneNumber = value;
                onPropertyChanged();

                if (string.IsNullOrEmpty(_phoneNumber))
                {
                    PhoneNumberValidation = "required";
                    _allowRegister = false;
                }

                else if (_phoneNumber.Length < 11)
                {
                    PhoneNumberValidation = "invalid mobile number";
                    _allowRegister = false;
                }

                else
                {
                    PhoneNumberValidation = string.Empty;
                    setRegProp();
                }
            }
        }

        public string HomeAddress
        {

            get => this._homeAddress;

            set
            {
                this._homeAddress = value;
                onPropertyChanged();

                if (string.IsNullOrEmpty(_homeAddress))
                {
                    AddressValidation = "required";
                    _allowRegister = false;
                }

                else
                {
                    AddressValidation = string.Empty;
                    setRegProp();
                }

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

        public RegistrationProfile newUserProfile { get; set; }
        /// <summary>
        /// Default Constructor..
        /// </summary>
        public RegistrationPageViewModel(RegistrationProfile registrationProfile)
        {
            UserRegisterCommand = new Command(RegisterUser);
            newUserProfile = registrationProfile;
        }

        #endregion


        private async void RegisterUser()
        {

            if (_allowRegister == false)
            {
                await Application.Current.MainPage.DisplayAlert("input error", "one or more input fields not set properly", "retry"); /// test
                return;
            }

            if (this.isSignUpInit == true)
                return;

            await Task.Run(async () =>
            {
                this.isSignUpInit = true;

                newUserProfile.HomeAddress = _homeAddress;
                newUserProfile.PhoneNumber = _phoneNumber;

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


        private bool checkFieldStates()
        {
            bool check = true;
            if (string.IsNullOrEmpty(_phoneNumber) == true || string.IsNullOrEmpty(_addressValidation) == true)
            {
                check = true;
            }

            else if (string.IsNullOrEmpty(_phoneNumber) == false && string.IsNullOrEmpty(_addressValidation) == false)
            {
                check = false;
            }
            return check;
        }

        private void setRegProp()
        {
            if (checkFieldStates() == false)
            {
                _allowRegister = true;
            }

            else
            {
                _allowRegister = false;
            }
        }

    }
}
