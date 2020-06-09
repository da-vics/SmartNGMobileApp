using SmartNG.DataProfiles;
using SmartNG.Views.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartNG
{
    class istRegistrationPageViewModel : BaseViewModel
    {

        #region PrivateMembers
        private string _fullName { get; set; }
        private string _Email { get; set; }
        private string _password { get; set; }
        private string _confirmPassword { get; set; }
        private bool _isNext { get; set; } = false;

        private bool _isallowedToMove { get; set; } = false;
        #endregion

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

                ///  var verifyEmail = new EmailValidator(Email);

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
        }

        private async void MoveToRegMain(object obj)
        {
            if (this._isNext == true)
                return;

            if (_isallowedToMove)
                await Application.Current.MainPage.Navigation.PushModalAsync(new RegistrationPage(newUserProfile));

            else
            {
                await Application.Current.MainPage.DisplayAlert("Fields Cannot be Null", "failed", "refill"); /// test
            }
        }

        #endregion




    }///
}
