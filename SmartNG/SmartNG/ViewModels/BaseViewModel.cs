using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartNG
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void onPropertyChanged([CallerMemberName]string name = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
