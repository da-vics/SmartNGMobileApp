﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartNG.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlPage : Shell
    {
        public ControlPage()
        {
            InitializeComponent();
        }


        ///   protected override bool OnBackButtonPressed() => true; /// to prevent back to loginPage
    }
}