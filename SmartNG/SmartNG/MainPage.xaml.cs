using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartNG
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _mainPageViewModel = new MainPageViewModel();

        public MainPage(string email = null, string password = null)
        {
            InitializeComponent();

            this.BindingContext = _mainPageViewModel;

            UEmail.Text = email;
            UPassword.Text = password;
        }


        protected async override void OnAppearing()
        {
            await _mainPageViewModel.startupInitChecks();
            base.OnAppearing();
        }


        protected override void OnDisappearing()
        {
            _mainPageViewModel.IsPageActive = false;
            base.OnDisappearing();
        }


    }
}
