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
        private ListServicePageViewModel listServicePageViewModel = new ListServicePageViewModel();
        public ListServicesPage()
        {
            InitializeComponent();

            BindingContext = listServicePageViewModel;

            ServiceList.ItemSelected += listServicePageViewModel.SelectedList;

        }

        protected override async void OnAppearing()
        {
            ///ObservableCollection
            ///
            await listServicePageViewModel.startupInitChecks();
            base.OnAppearing();
        }

    }
}