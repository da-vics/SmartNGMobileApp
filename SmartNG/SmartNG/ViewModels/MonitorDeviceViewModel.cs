using Java.Lang;
using SmartNG.DataProfiles;
using SmartNG.RestAPIClientHandlers;
using SmartNG.RestAPIClientHandlers.CustomExceptions;
using Syncfusion.SfGauge.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartNG
{
    public class MonitorDeviceViewModel : BaseViewModel
    {
        private bool _isPageActive { get; set; } = false;

        private bool _noSerivceData { get; set; } = false;

        private GetServicesProfile _servicesProfile { get; set; }

        private ServicesDataProfile _servicesData { get; set; } = null;

        public bool isUpdate { set; get; } = false;

        public string HeaderText { get => _servicesProfile.serviceName; }

        private double _value { get; set; } = 0;

        private string _noServiceMessage1 { get; set; } = string.Empty;
        private string _noServiceMessage2 { get; set; } = string.Empty;



        public bool NoservicesData
        {
            get => _noSerivceData;

            set
            {
                _noSerivceData = value;
                onPropertyChanged();

            }
        }

        public string NoServiceMessage1
        {

            get => _noServiceMessage1;

            set
            {
                _noServiceMessage1 = value;
                onPropertyChanged();
            }

        }

        public string NoServiceMessage2
        {

            get => _noServiceMessage2;

            set
            {
                _noServiceMessage2 = value;
                onPropertyChanged();
            }

        }

        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (_value != value)
                {
                    _value = value;
                    onPropertyChanged();
                }
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

        public MonitorDeviceViewModel(GetServicesProfile servicesProfile)
        {
            _servicesProfile = servicesProfile;
        }

        public async Task startupInitChecks()
        {
            IsPageActive = false;
            NoservicesData = false;
            NoServiceMessage1 = string.Empty;
            NoServiceMessage2 = string.Empty;

            // await Task.Run(async () =>
            //{
            //    try
            //    {
            //        await UpdateData();
            //        NoservicesData = false;
            //    }

            //    catch (SmartNgHttpException args)
            //    {
            //        NoservicesData = true;
            //        NoServiceMessage1 = "No Data Yet";
            //        NoServiceMessage2 = "make sure device is setup!";
            //        Console.WriteLine(args.Message);
            //    }



            //});

            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Run(async () =>
                    {

                        try
                        {
                            await UpdateData();
                            NoservicesData = false;
                        }
                        catch (SmartNgHttpException args)
                        {
                            NoservicesData = true;
                            NoServiceMessage1 = "No Data Yet";
                            NoServiceMessage2 = "make sure device is setup!";
                            Console.WriteLine(args.Message);
                        }

                        IsPageActive = true; /// test

                    });
                }

            });

        }

        private async Task UpdateData()
        {
            var getServiceData = new GetServiceDataApi(_servicesProfile);


            try
            {
                _servicesData = await getServiceData.GetUserServices();

            }

            catch (SmartNgHttpException)
            {
                throw;
            }


            if (_servicesData != null && _servicesData.userdata != null)
            {
                try
                {
                    Value = double.Parse(_servicesData.userdata);
                }

                catch (FormatException args)
                {
                    Console.WriteLine(args.Message);
                }
            }

        }
    }
}
