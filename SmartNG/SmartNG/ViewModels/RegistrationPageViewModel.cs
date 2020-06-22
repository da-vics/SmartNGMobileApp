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

        private string _phoneNumberValidation { get; set; }
        private string _addressValidation { get; set; }

        private bool _hasPhoneNumberError { get; set; } = true;
        private bool _hasAddressError { get; set; } = true;

        private bool _allowRegister { get; set; } = false;

        private bool _IsRegSuccessful { get; set; } = false;

        private bool _isPageActive { get; set; } = false;
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

        public bool HasPhoneNumberError
        {
            get => _hasPhoneNumberError;

            set
            {
                _hasPhoneNumberError = value;
                onPropertyChanged();
            }
        }

        public bool HasAddressError
        {

            get => _hasAddressError;

            set
            {
                _hasAddressError = value;
                onPropertyChanged();
            }

        }

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
                    HasPhoneNumberError = true;
                    _allowRegister = false;
                }

                else if (_phoneNumber.Length < 11)
                {
                    PhoneNumberValidation = "invalid mobile number";
                    _allowRegister = false;
                    HasPhoneNumberError = true;
                }

                else
                {
                    PhoneNumberValidation = string.Empty;
                    HasPhoneNumberError = false;
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
                    HasAddressError = true;
                }

                else
                {
                    AddressValidation = string.Empty;
                    HasAddressError = false;
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
            UserRegisterCommand = new Command(async () => await RegisterUser());
            newUserProfile = registrationProfile;
            PhoneNumber = string.Empty;
            HomeAddress = null;
            PhoneNumberValidation = "required";
            AddressValidation = "required";
        }

        #endregion


        private async Task RegisterUser()
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

                if (!string.IsNullOrEmpty(_homeAddress))
                {
                    _homeAddress = _homeAddress.Trim();
                }

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

                Application.Current.MainPage = new NavigationPage(new MainPage(newUserProfile.Email, newUserProfile.PassWordHash));
            }////

            else
            {
                _IsRegSuccessful = false;
                await Application.Current.MainPage.DisplayAlert("test error", "Failed", "Retry");
            } ///
        }


        private bool checkFieldStates()
        {
            bool check = string.IsNullOrEmpty(PhoneNumberValidation);
            check &= string.IsNullOrEmpty(AddressValidation);

            return check;
        }

        private void setRegProp()
        {
            if (checkFieldStates() == true)
            {
                _allowRegister = true;
            }

            else
            {
                _allowRegister = false;
            }
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
