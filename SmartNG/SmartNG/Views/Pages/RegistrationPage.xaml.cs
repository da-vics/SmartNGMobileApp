using SmartNG.DataProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        private RegistrationPageViewModel _registrationPageViewModel { get; set; } = null;

        public RegistrationPage(RegistrationProfile reguser)
        {
            InitializeComponent();
            _registrationPageViewModel = new RegistrationPageViewModel(reguser);
            BindingContext = _registrationPageViewModel;
        }

        protected async override void OnAppearing()
        {
            await _registrationPageViewModel.startupInitChecks();
            base.OnAppearing();
        }


        protected override void OnDisappearing()
        {
            _registrationPageViewModel.IsPageActive = false;
            base.OnDisappearing();
        }


    }
}