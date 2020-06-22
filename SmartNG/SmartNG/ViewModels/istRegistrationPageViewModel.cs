using Java.Lang;
using SmartNG.DataProfiles;
using SmartNG.Helpers;
using SmartNG.Views.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartNG
{
    class istRegistrationPageViewModel : BaseViewModel
    {

        #region PrivateMembers
        private string _fullName { get; set; } = string.Empty;
        private string _Email { get; set; } = string.Empty;
        private string _password { get; set; } = string.Empty;
        private string _confirmPassword { get; set; } = string.Empty;
        private bool _isNext { get; set; } = false;

        private string _nameValidation { get; set; }
        private string _emailValidation { get; set; }
        private string _passwordValidation { get; set; }
        private string _confirmPassValidation { get; set; }

        private bool _hasEmailError { get; set; } = true;
        private bool _hasPasswordError { get; set; } = true;
        private bool _hasFullnameError { get; set; } = true;
        private bool _hasConfirmPasswordError { get; set; } = true;

        private bool checkDone { get; set; } = false;

        private bool IsallowedToMove { get; set; } = false;

        private bool _isPageActive { get; set; } = false;
        #endregion

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

        public bool HasFullNameError
        {
            get => _hasFullnameError;

            set
            {
                _hasFullnameError = value;
                onPropertyChanged();
            }
        }

        public bool HasConfirmPasswordError
        {

            get => _hasConfirmPasswordError;

            set
            {
                _hasConfirmPasswordError = value;
                onPropertyChanged();
            }
        }

        #region EntryValidations

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

        public string ConfirmPassValidation
        {
            get => _confirmPassValidation;

            set
            {
                this._confirmPassValidation = value;
                onPropertyChanged();

            }
        }

        public string NameValidation
        {
            get => _nameValidation;

            set
            {
                this._nameValidation = value;
                onPropertyChanged();
            }
        }

        #endregion

        public string FullName
        {
            get => this._fullName;

            set
            {
                this._fullName = value;

                _fullName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_fullName);

                ///// System.Globalization.CultureInfo.TextInfo.ToTitleCase
                //_fullName = _fullName.ToUpperInvariant();  /// test

                onPropertyChanged();

                var temp = _fullName;

                if (!string.IsNullOrEmpty(_fullName))
                {
                    temp = _fullName.Trim();
                }


                if (string.IsNullOrEmpty(_fullName))
                {
                    NameValidation = "required";
                    IsallowedToMove = false;
                    HasFullNameError = true;
                }

                else if (!temp.Contains(" "))
                {
                    NameValidation = "full name required";
                    IsallowedToMove = false;
                    HasFullNameError = true;
                }

                else
                {
                    NameValidation = string.Empty;
                    HasFullNameError = false;
                    setRegProp();
                }

            }
        }

        public string Email
        {

            get => this._Email;

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
                    IsallowedToMove = false;
                    HasEmailError = true;
                }

                else if (!EmailValidator.IsValid(_Email))
                {
                    EmailValidation = "invalid email";
                    IsallowedToMove = false;
                    HasEmailError = true;
                }

                else
                {
                    EmailValidation = string.Empty;
                    HasEmailError = false;
                    setRegProp();
                }
            }
        }

        public string Password
        {

            get => this._password;

            set
            {
                this._password = value;
                onPropertyChanged();

                if (string.IsNullOrEmpty(_password))
                {
                    PasswordValidation = "required";
                    IsallowedToMove = false;
                    HasPasswordError = true;
                }

                else if (_password.Length < 6)
                {
                    PasswordValidation = "too short";
                    IsallowedToMove = false;
                    HasPasswordError = true;
                }

                else
                {
                    PasswordValidation = string.Empty;
                    HasPasswordError = false;
                    setRegProp();
                }
            }
        }

        public string ConfirmPassword
        {

            get => this._confirmPassword;

            set
            {
                this._confirmPassword = value;
                onPropertyChanged();

                if (!string.IsNullOrEmpty(_password) && _confirmPassword != _password)
                {
                    ConfirmPassValidation = "password must match";
                    IsallowedToMove = false;
                    HasConfirmPasswordError = true;
                    checkDone = false;
                }

                else if (string.IsNullOrEmpty(_confirmPassword))
                {
                    ConfirmPassValidation = "required";
                    HasConfirmPasswordError = true;
                    IsallowedToMove = false;
                    checkDone = false;
                }

                else
                {
                    ConfirmPassValidation = string.Empty;
                    checkDone = true;
                    HasConfirmPasswordError = false;
                    setRegProp();
                }
            }

        }

        public bool isNextInit
        {
            get => this._isNext;

            set
            {
                this._isNext = value;
                onPropertyChanged();
            }
        }
        #endregion


        public ICommand UserSetRegCommand { get; private set; }

        public RegistrationProfile newUserProfile { get; set; }

        #region Default Constructor
        public istRegistrationPageViewModel()
        {
            UserSetRegCommand = new Command(async (object obj) => await MoveToRegMain(obj));
            newUserProfile = new RegistrationProfile();
            isNextInit = false;
            NameValidation = "required";
            EmailValidation = "required";
            ConfirmPassValidation = "required";
            PasswordValidation = "required";
        }
        #endregion


        private async Task MoveToRegMain(object obj)
        {

            if (IsallowedToMove == false)
            {
                await Application.Current.MainPage.DisplayAlert("input error", "one or more input fields not set properly", "retry"); /// test
                return;
            }

            if (isNextInit == true)
                return;

            isNextInit = true;

            if (!string.IsNullOrEmpty(_fullName))
            {
                _fullName = _fullName.Trim();
            }

            #region CreatesUserProfile 
            newUserProfile.Fullname = _fullName;
            newUserProfile.Email = _Email;
            newUserProfile.PassWordHash = _password;
            #endregion

            if (IsallowedToMove)
                await Application.Current.MainPage.Navigation.PushModalAsync(new RegistrationPage(newUserProfile));

            isNextInit = false;
        }

        private bool checkFieldStates()
        {
            bool check = string.IsNullOrEmpty(_emailValidation);
            check &= string.IsNullOrEmpty(_passwordValidation);
            check &= string.IsNullOrEmpty(_confirmPassValidation);
            check &= string.IsNullOrEmpty(_nameValidation);

            return check;
        }

        private void setRegProp()
        {
            if (checkFieldStates() == true)
            {
                IsallowedToMove = true;
            }
            else
            {
                IsallowedToMove = false;
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

    }///
}
