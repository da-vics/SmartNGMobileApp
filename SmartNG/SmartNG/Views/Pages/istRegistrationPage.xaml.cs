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
    public partial class istRegistrationPage : ContentPage
    {
        private istRegistrationPageViewModel _istRegistrationPageViewModel = new istRegistrationPageViewModel();

        public istRegistrationPage()
        {
            InitializeComponent();
            BindingContext = _istRegistrationPageViewModel;
        }


        protected async override void OnAppearing()
        {
            await _istRegistrationPageViewModel.startupInitChecks();
            base.OnAppearing();
        }


        protected override void OnDisappearing()
        {
            _istRegistrationPageViewModel.IsPageActive = false;
            base.OnDisappearing();
        }

    }
}