using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartNG.ViewModels
{
    public class ScanQRCodeViewModel : BaseViewModel
    {

        private bool _isPageActive { get; set; } = false;

        public bool IsPageActive
        {
            get => _isPageActive;

            set
            {
                _isPageActive = value;
                onPropertyChanged();

            }
        }

        public ScanQRCodeViewModel()
        {

        }

        public async Task startupInitChecks()
        {
            IsPageActive = false;

            await Task.Run(async () =>
            {
                await Task.Delay(1500);
                IsPageActive = true;

            });
        }

    }
}
