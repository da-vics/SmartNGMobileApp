using SmartNG.DataProfiles;
using SmartNG.RestAPIClientHandlers;
using SmartNG.Views.Pages.ControlPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartNG
{
    public class AddServiceViewModel : BaseViewModel
    {
        #region PrivateMembers
        private string _serviceName { get; set; } = string.Empty;
        private string _cylinderWeight { get; set; } = null;
        private string _deviceId { get; set; } = string.Empty;
        private bool _isAddService { get; set; } = false;

        private bool _isAddServiceSuccessful { get; set; } = false;

        private string _serviceValidation { get; set; } = "required";
        private string _weightValidation { get; set; } = "required";
        private string _deviceIdValidation { get; set; } = "tap QRImage to scan QRCode";

        private bool _hasServiceNameError { get; set; } = true;
        private bool _hasWeightError { get; set; } = true;
        private bool _hasDeviceIdError { get; set; } = true;

        #endregion

        #region PublicAccessors

        public bool HasServiceNameError
        {
            get => _hasServiceNameError;

            set
            {
                _hasServiceNameError = value;
                onPropertyChanged();
            }
        }

        public bool HasDeviceIdError
        {
            get => _hasDeviceIdError;

            set
            {
                _hasDeviceIdError = value;
                onPropertyChanged();
            }
        }

        public bool HasWeightError
        {

            get => _hasWeightError;

            set
            {
                _hasWeightError = value;
                onPropertyChanged();
            }
        }

        public string ServiceValidation
        {
            get => _serviceValidation;

            set
            {
                _serviceValidation = value;
                onPropertyChanged();
            }
        }

        public string WeightValidation
        {
            get => _weightValidation;

            set
            {
                _weightValidation = value;
                onPropertyChanged();
            }
        }


        public string DeviceIdValidation
        {
            get => _deviceIdValidation;

            set
            {
                _deviceIdValidation = value;
                onPropertyChanged();
            }
        }


        public bool isAddService
        {
            get => _isAddService;

            set
            {
                _isAddService = value;
                onPropertyChanged();
            }
        }

        public string ServiceName
        {
            get => _serviceName;

            set
            {
                _serviceName = value;

                if (!string.IsNullOrEmpty(_serviceName))
                {
                    _serviceName = _serviceName.Trim();
                }

                onPropertyChanged();

                if (string.IsNullOrEmpty(_serviceName))
                {
                    ServiceValidation = "required";
                    HasServiceNameError = true;
                }

                else if (_serviceName.Length < 5)
                {
                    ServiceValidation = "Too Short";
                    HasServiceNameError = true;
                }

                else
                {
                    ServiceValidation = string.Empty;
                    HasServiceNameError = false;
                }
            }

        }

        public string CylinderWeight
        {
            get => _cylinderWeight;

            set
            {
                _cylinderWeight = value;

                onPropertyChanged();

                if (string.IsNullOrEmpty(_cylinderWeight))
                {
                    WeightValidation = "required";
                    HasWeightError = true;
                }

                else if (int.Parse(_cylinderWeight) == 0)
                {
                    WeightValidation = "field can't be zero";
                    HasWeightError = true;
                }

                else
                {
                    WeightValidation = string.Empty;
                    HasWeightError = false;
                }
            }

        }


        public string DeviceId
        {
            get => _deviceId;

            set
            {
                _deviceId = value;

                onPropertyChanged();

                if (string.IsNullOrEmpty(_deviceId))
                {
                    DeviceIdValidation = "tap QRImage to scan QRCode";
                    HasDeviceIdError = true;
                }

                else
                {
                    DeviceIdValidation = string.Empty;
                    HasDeviceIdError = false;
                }
            }///


        }

        #endregion
        public ICommand ScanQRCodeCommand { get; private set; }

        public ICommand AddNewServiceCommand { get; private set; }

        public AddServiceViewModel(string deviceId = null)
        {
            isAddService = false;
            ScanQRCodeCommand = new Command(async () => await scanQRCode());
            AddNewServiceCommand = new Command(async () => await AddNewService());

            if (!string.IsNullOrEmpty(deviceId))
                DeviceId = deviceId;
        }

        private async Task scanQRCode()
        {
            await Shell.Current.Navigation.PushModalAsync(new QRScannerPage(this));
        }


        private bool checkFieldStates()
        {
            bool check = string.IsNullOrEmpty(ServiceValidation)
                & string.IsNullOrEmpty(DeviceIdValidation)
                & string.IsNullOrEmpty(WeightValidation);

            return check;
        }

        private async Task AddNewService()
        {
            if (!checkFieldStates())
            {
                await Application.Current.MainPage.DisplayAlert("input error", "one or more input fields not set properly", "retry"); /// test
            }

            if (isAddService)
                return;


            await Task.Run(async () =>
             {
                 if (Application.Current.Properties.ContainsKey("ApiKey"))
                 {
                     isAddService = true;
                     var Apikey = Application.Current.Properties["ApiKey"].ToString();

                     #region New UserService
                     var UserSerivceProfile = new AddServiceProfile
                     {
                         apiKey = Apikey,
                         servicename = this._serviceName,
                         DeviceId = this._deviceId,
                         DeviceType = (short)Int16.Parse(this._cylinderWeight)
                     };
                     #endregion

                     var addUserService = new AddUserServiceApi(UserSerivceProfile);

                     _isAddServiceSuccessful = await addUserService.addNewService();

                     isAddService = false;
                 }///

                 else
                 {

                 }

             });

            if (_isAddServiceSuccessful)
            {
                DeviceId = string.Empty;
                ServiceName = string.Empty;
                CylinderWeight = string.Empty;

                await Application.Current.MainPage.DisplayAlert("Successful", "new service added", "Ok");
            }


            else
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Try Again", "Retry");
            }


        }
    }


}
