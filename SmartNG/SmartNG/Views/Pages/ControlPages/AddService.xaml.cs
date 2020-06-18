using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG.Views.Pages.ControlPages
{
    [QueryProperty(nameof(Deviceid), nameof(Deviceid))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddServicePage : ContentPage
    {
        private string deviceid = string.Empty;

        public string Deviceid
        {
            get => deviceid;

            set
            {
                deviceid = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged(nameof(Deviceid));

                addServiceViewModel.DeviceId = deviceid;
            }

        }

        AddServiceViewModel addServiceViewModel { get; set; } = new AddServiceViewModel();

        public AddServicePage()
        {
            InitializeComponent();
            BindingContext = addServiceViewModel;
        }

    }

}