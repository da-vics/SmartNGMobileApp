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

        private bool checkDone { get; set; } = false;

        private bool IsallowedToMove { get; set; } = false;
        #endregion

        #region Accessors

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
                }

                else if (!temp.Contains(" "))
                {
                    NameValidation = "full name required";
                    IsallowedToMove = false;
                }

                else
                {
                    NameValidation = string.Empty;
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
                }

                else if (!EmailValidator.IsValid(_Email))
                {
                    EmailValidation = "invalid email";
                    IsallowedToMove = false;
                }

                else
                {
                    EmailValidation = string.Empty;
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
                }

                else if (_password.Length < 6)
                {
                    PasswordValidation = "too short";
                    IsallowedToMove = false;
                }

                else
                {
                    PasswordValidation = string.Empty;
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
                    checkDone = false;
                }

                else if (string.IsNullOrEmpty(_confirmPassword))
                {
                    ConfirmPassValidation = "required";
                    IsallowedToMove = false;
                    checkDone = false;
                }

                else
                {
                    ConfirmPassValidation = string.Empty;
                    checkDone = true;
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
            UserSetRegCommand = new Command(MoveToRegMain);
            newUserProfile = new RegistrationProfile();
            isNextInit = false;
            NameValidation = "required";
            EmailValidation = "required";
            ConfirmPassValidation = "required";
            PasswordValidation = "required";
        }
        #endregion


        private async void MoveToRegMain(object obj)
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

    }///
}
