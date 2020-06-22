using Java.Lang;
using SmartNG.DataProfiles;
using Syncfusion.SfGauge.XForms;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG.Views.Pages.ControlPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonitorDevicesPage : ContentPage
    {
        private MonitorDeviceViewModel _monitorDevice = null;
        public MonitorDevicesPage(GetServicesProfile servicesProfile)
        {
            InitializeComponent();
            _monitorDevice = new MonitorDeviceViewModel(servicesProfile);

            BindingContext = _monitorDevice;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _monitorDevice.startupInitChecks();

        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }


    }
}