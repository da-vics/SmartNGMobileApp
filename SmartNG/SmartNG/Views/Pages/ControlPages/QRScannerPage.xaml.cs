using SmartNG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG.Views.Pages.ControlPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRScannerPage : ContentPage
    {
        private AddServiceViewModel _addService { get; set; }
        private ScanQRCodeViewModel scanQR { get; set; } = new ScanQRCodeViewModel();

        public QRScannerPage(AddServiceViewModel addService)
        {

            InitializeComponent();
            scanView.AutoFocus();

            _addService = addService;

            BindingContext = scanQR;
            //scanView.Options.PossibleFormats.Clear();
            //scanView.Options.PossibleFormats = new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.CODE_128  };

        }

        protected override async void OnAppearing()
        {
            await scanQR.startupInitChecks();
            ///   await _listServicePageViewModel.startupInitChecks();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            scanQR.IsPageActive = false;
            base.OnDisappearing();
        }


        private void scanView_OnScanResult(ZXing.Result result)
        {
            scanView.IsScanning = false;
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (result.BarcodeFormat != ZXing.BarcodeFormat.QR_CODE)
                {
                    await Application.Current.MainPage.DisplayAlert("Scanned result", "Only QR Code Allowed!", "OK");

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Scanned result", result.Text, "OK");   /// test
                    scanView.IsScanning = true;

                }
                /// await Shell.Current.GoToAsync($"{nameof(AddServicePage)}?Deviceid={result.Text}");

                _addService.DeviceId = result.Text;
                await Shell.Current.Navigation.PopModalAsync();

            });

        }
    }
}