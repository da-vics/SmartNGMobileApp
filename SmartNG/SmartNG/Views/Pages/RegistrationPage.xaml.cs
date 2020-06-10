using SmartNG.DataProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage(RegistrationProfile reguser)
        {
            InitializeComponent();
            this.BindingContext = new RegistrationPageViewModel(reguser);
        }

        //private void Editor_Completed(object sender, EventArgs e)
        //{

        //}
    }
}