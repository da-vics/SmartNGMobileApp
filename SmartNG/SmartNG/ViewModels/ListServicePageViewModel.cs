using Java.Lang;
using SmartNG.DataProfiles;
using SmartNG.RestAPIClientHandlers;
using SmartNG.RestAPIClientHandlers.CustomExceptions;
using SmartNG.Views.Pages.ControlPages;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartNG
{
    class ListServicePageViewModel : BaseViewModel
    {
        private bool _isPageActive { get; set; } = false;

        private bool _noSerivces { get; set; } = false;

        private bool _isRefreshing { get; set; } = false;

        private bool _isPullToRefresh { get; set; } = false;




        private ObservableCollection<GetServicesProfile> _userServices { get; set; } = null;


        #region Accessors
        public ObservableCollection<GetServicesProfile> UserServices
        {
            get => _userServices;

            set
            {
                _userServices = value;
                onPropertyChanged();

            }

        }


        public bool IsRefreshing
        {
            get => _isRefreshing;

            set
            {
                _isRefreshing = value;
                onPropertyChanged();

            }
        }

        public bool IsPullToRefresh
        {
            get => _isPullToRefresh;

            set
            {
                _isPullToRefresh = value;
                onPropertyChanged();

            }
        }

        public bool Noservices
        {
            get => _noSerivces;

            set
            {
                _noSerivces = value;
                onPropertyChanged();

            }
        }

        public bool IsPageActive
        {
            get => _isPageActive;

            set
            {
                _isPageActive = value;
                onPropertyChanged();

            }
        }

        #endregion

        public ICommand AddServiceCommand { get; private set; }

        public ICommand RefreshListCommand { get; private set; }

        public ListServicePageViewModel()
        {
            Noservices = false;
            AddServiceCommand = new Command(async () => await AddNewUserService());

        }

        public async Task AddNewUserService()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddServicePage());
        }

        public async Task startupInitChecks()
        {

            IsPageActive = false;

            await Task.Run(async () =>
            {
                var serviceapi = new GetUserServicesApi();

                try
                {
                    UserServices = await serviceapi.GetUserServices();

                    IsPageActive = true;
                }

                catch (SmartNgHttpException args)
                {
                    IsPageActive = true;
                    Noservices = true;
                    Console.WriteLine(args.Message);
                }

            });
        }

        public void SelectedList(object sender, SelectedItemChangedEventArgs e)
        {
            var servicesProfile = (GetServicesProfile)e.SelectedItem;
            Application.Current.MainPage.Navigation.PushModalAsync(new MonitorDevicesPage(servicesProfile));

        }

    }
}
