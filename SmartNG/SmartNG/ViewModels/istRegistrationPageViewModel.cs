using Java.Lang;
using SmartNG.DataProfiles;
using SmartNG.Helpers;
using SmartNG.Views.Pages;
using System;
using System.Collections.Generic;
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

        private string _nameValidation { get; set; } = "required";
        private string _emailValidation { get; set; } = "required";
        private string _passwordValidation { get; set; } = "required";
        private string _confirmPassValidation { get; set; } = "required";

        private Color _proceedBtnColor { get; set; } = Color.DarkGray;

        private bool _isallowedToMove { get; set; } = false;
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

        public Color ProceedBtnColor
        {
            get => _proceedBtnColor;

            set
            {
                _proceedBtnColor = value;
                onPropertyChanged();
            }
        }
        #endregion

        public bool IsallowedToMove
        {
            get => _isallowedToMove;

            set
            {
                _isallowedToMove = value;
                onPropertyChanged();
            }

        }

        public string FullName
        {

            get => this._fullName;

            set
            {
                this._fullName = value;
                if (!string.IsNullOrEmpty(_fullName))
                {
                    _fullName = _fullName.Trim();
                }
                onPropertyChanged();


                if (string.IsNullOrEmpty(_fullName))
                {
                    NameValidation = "required";
                    ProceedBtnColor = Color.DarkGray;
                    IsallowedToMove = false;
                }

                else if (!_fullName.Contains(" "))
                {
                    NameValidation = "full name required";
                    ProceedBtnColor = Color.DarkGray;
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
                onPropertyChanged();

                if (!string.IsNullOrEmpty(_Email) && EmailValidator.IsValid(_Email))
                {
                    EmailValidation = string.Empty;
                    setRegProp();
                }

                else
                {
                    EmailValidation = "required";
                    ProceedBtnColor = Color.DarkGray;
                    IsallowedToMove = false;
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
                    ProceedBtnColor = Color.DarkGray;
                    IsallowedToMove = false;
                }

                else if (_password.Length < 6)
                {
                    PasswordValidation = "too short";
                    ProceedBtnColor = Color.DarkGray;
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
                    ProceedBtnColor = Color.DarkGray;
                    IsallowedToMove = false;
                }

                else if (string.IsNullOrEmpty(_confirmPassword))
                {
                    ConfirmPassValidation = "required";
                    ProceedBtnColor = Color.DarkGray;
                    IsallowedToMove = false;
                }

                else
                {
                    ConfirmPassValidation = string.Empty;
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
        }

        private async void MoveToRegMain(object obj)
        {
            if (isNextInit == true)
                return;

            isNextInit = true;

            #region CreatesUserProfile 
            newUserProfile.Fullname = _fullName;
            newUserProfile.Email = _Email;
            newUserProfile.PassWordHash = _password;
            #endregion

            _isallowedToMove = true; /// test


            if (_isallowedToMove)
                await Application.Current.MainPage.Navigation.PushModalAsync(new RegistrationPage(newUserProfile));

            else
            {
                await Application.Current.MainPage.DisplayAlert("Fields Cannot be Null", "failed", "refill"); /// test
            }
            isNextInit = false;
        }

        #endregion

        private void setRegProp()
        {
            bool check = string.IsNullOrEmpty(_Email) &
                string.IsNullOrEmpty(_password) &
                string.IsNullOrEmpty(ConfirmPassword) &
                string.IsNullOrEmpty(FullName);

            if (check == false)
            {
                IsallowedToMove = true;
                ProceedBtnColor = Color.DarkRed;
            }

            else
            {
                ProceedBtnColor = Color.DarkGray;
                IsallowedToMove = false;
            }
        }


    }///
}
