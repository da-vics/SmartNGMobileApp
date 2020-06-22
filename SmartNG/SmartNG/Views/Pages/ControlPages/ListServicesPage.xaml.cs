using SmartNG.DataProfiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG.Views.Pages.ControlPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListServicesPage : ContentPage
    {
        private ListServicePageViewModel _listServicePageViewModel = new ListServicePageViewModel();
        public ListServicesPage()
        {
            InitializeComponent();

            BindingContext = _listServicePageViewModel;

            ServiceList.ItemSelected += _listServicePageViewModel.SelectedList;

        }

        protected override async void OnAppearing()
        {

            await _listServicePageViewModel.startupInitChecks();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _listServicePageViewModel.IsPageActive = false;
            base.OnDisappearing();
        }

    }
}